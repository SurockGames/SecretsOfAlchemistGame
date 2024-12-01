using UnityEngine;

namespace Assets.Code.Items
{
    public class GatherPopupService : MonoBehaviour
    {
        [SerializeField] private GatherPopup gatherPopup;
        [SerializeField] private RectTransform gatherPopupGroup;

        public void CreateGatherPopup(Item item, int amount)
        {
            var popup = Instantiate(gatherPopup, gatherPopupGroup);
            popup.SetPopup(item, amount);
        }
    }
}