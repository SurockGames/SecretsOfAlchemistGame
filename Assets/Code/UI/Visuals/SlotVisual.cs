using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code
{
    public class SlotVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private bool selfInitialize = false;
        public string name => Slot.EquipedItem ? Slot.EquipedItem.Name : "Empty Slot";
        public string description => Slot.EquipedItem ? Slot.EquipedItem.Description : "Find something to put here!";

        public event Action<SlotVisual> OnSlotHovered;
        public event Action<SlotVisual> OnSlotUnhovered;
        public event Action<SlotVisual> OnSlotClicked;

        public bool CanBeTransfered;
        public Slot Slot => slot;

        public int SlotIndex => slotIndex;
        private int slotIndex;
        private Color defaultColor;
        private Color defaultOutlineColor;
        private Sprite defaultSprite;
        private Outline outline;
        private TMP_Text amountText;
        private Image slotIcon;
        private Slot slot;

        private void Awake()
        {
            if (selfInitialize)
            {
                Initialize(0);
            }
        }

        public void Initialize(int id)
        {
            slotIndex = id;

            slotIcon = GetComponent<Image>();
            outline = GetComponent<Outline>();
            amountText = GetComponentInChildren<TMP_Text>();

            slot = GetComponent<Slot>();

            defaultColor = slotIcon.color;
            defaultSprite = slotIcon.sprite;
            defaultOutlineColor = outline.effectColor;
        }

        public void SetItem(Item item)
        {
            if (slot)
            {
                slot.EquipItem(item);
                SetIcon(item.Icon);
            }
            //this.item = item;
        }

        public void SetIcon(Sprite sprite)
        {
            slotIcon.sprite = sprite;
            slotIcon.color = Color.white;
        }

        public void SetAmount(int amount)
        {
            if (slot)
            {
                slot.SetAmount(amount);
            }
            amountText.enabled = true;
            amountText.text = $"x{amount}";
        }

        public void ClearToDefault()
        {
            slotIcon.color = defaultColor;
            slotIcon.sprite = defaultSprite;
            amountText.enabled = false;

            Slot.ClearSlot();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSlotHovered?.Invoke(this);

            outline.effectColor = Color.white;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnSlotUnhovered?.Invoke(this);

            outline.effectColor = defaultOutlineColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnSlotClicked?.Invoke(this);
        }

        public void Select()
        {

        }

    }
}