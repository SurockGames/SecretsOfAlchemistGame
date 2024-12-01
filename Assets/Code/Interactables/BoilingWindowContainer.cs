using UnityEngine;

namespace Assets.Code
{
    public class BoilingWindowContainer : WindowContainer
    {
        [SerializeField] private BoilingCraftWindow window;

        public override void Activate()
        {
            windowsService.ActivatePlayerInventoryAsLeftWindow(true, false, false);
            windowsService.ActivateRightWindow(window);
        }
    }
}