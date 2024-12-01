using Assets.Code.Shooting;
using UnityEngine;

namespace Assets.Code.Items
{
    public class ItemGrabber : MonoBehaviour
    {
        [SerializeField] private PlayerInventoryService inventory;
        [SerializeField] private GatherPopupService gatherPopupService;

        public bool TakeItem(Item item, int amount)
        {
            if (inventory.TryAddItemToInventory(item, amount))
            {
                gatherPopupService.CreateGatherPopup(item, amount);
                return true;
            }
            return false;
        }
    }

    public class GunGrabber : MonoBehaviour
    {
        [SerializeField] private PlayerHandsService hands;
        [SerializeField] private GatherPopupService gatherPopupService;

        public bool TakeItem(Item item, int amount)
        {
            //if (hands.TryAddItemToInventory(item, amount))
            //{
            //    gatherPopupService.CreateGatherPopup(item, amount);
            //    return true;
            //}
            return false;
        }
    }
}