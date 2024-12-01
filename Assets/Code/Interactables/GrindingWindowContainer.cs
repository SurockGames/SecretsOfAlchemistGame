using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Code
{
    public class GrindingWindowContainer : WindowContainer
    {
        [SerializeField] private GrindingCraftWindow window;

        public override void Activate()
        {
            windowsService.ActivatePlayerInventoryAsLeftWindow(true, false, false);
            windowsService.ActivateRightWindow(window);
        }
    }
}