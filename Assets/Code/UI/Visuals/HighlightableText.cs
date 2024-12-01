using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code.Items
{
    public class HighlightableText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool IsScalableOnHover;
        public float fontHoverScaleAmount;
        public Color hoverColor;

        public event Action OnClick;

        private TMP_Text text;
        private Color defaultColor;
        private bool isActive;
        private float defaultSize;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            defaultColor = text.color;

            defaultSize = text.fontSize;
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                this.isActive = true;
                Highlight();
            }
            else
            {
                this.isActive = false;
                StopHighlighting();
            }
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isActive)
                Highlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isActive)
                StopHighlighting();
        }

        private void Highlight()
        {
            text.color = Color.white;

            if (IsScalableOnHover)
            {
                text.fontSize = defaultSize + fontHoverScaleAmount;
            }
        }

        private void StopHighlighting()
        {
            text.color = defaultColor;

            if (IsScalableOnHover)
            {
                text.fontSize = defaultSize - fontHoverScaleAmount;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}