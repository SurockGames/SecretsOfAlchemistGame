using UnityEngine;

namespace Assets.Code
{
    public class PotionCraftWindowContainer : WindowContainer
    {
        [SerializeField] private ComplexCraftWindow window;

        public override void Activate()
        {
            windowsService.ActivatePlayerInventoryAsLeftWindow(true, true, false);
            windowsService.ActivateRightWindow(window);
        }
    }
}