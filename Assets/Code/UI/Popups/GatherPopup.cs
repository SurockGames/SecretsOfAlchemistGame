using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;

namespace Assets.Code.Items
{
    public class GatherPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private SlotVisual slot;
        [SerializeField] private RectTransform container;
        [SerializeField] private RectMask2D containerMask;
        [SerializeField] private float displayDuration;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float lifeTime;

        private float velocity;
        private bool isUpdated;

        public void SetPopup(Item item, int amount)
        {
            container.DOAnchorPosX(0, displayDuration);


            itemNameText.text = item.name;
            slot.Initialize(0);
            slot.SetAmount(amount);
            slot.SetIcon(item.Icon);

            StartCoroutine(FadeContainerMask(0));

            StartCoroutine(KillSelf(lifeTime));
        }

        private void Update()
        {
            isUpdated = true;
        }

        private IEnumerator FadeContainerMask(float to)
        {
            while (containerMask.softness.x > to)
            {
                containerMask.softness = new Vector2Int((int)Mathf.SmoothDamp(containerMask.softness.x, to, ref velocity, Time.fixedDeltaTime, maxSpeed), 0);
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator KillSelf(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(gameObject);
        }
    }


}