using UnityEngine;

namespace Assets.Code
{
    public class PlayerInventoryWindowContainer : WindowContainer
    {
        [SerializeField] private PlayerInventoryWindow playerInventoryWindow;

        [SerializeField] private PlayerActiveMenuType activeMenuType;

        [SerializeField] private GunInventoryWindow gunInventoryWindow;
        [SerializeField] protected PotionInventoryWindow potionInventoryWindow;

        public override void Activate()
        {
            switch (activeMenuType)
            {
                case PlayerActiveMenuType.None:
                    windowsService.ActivatePlayerInventoryAsLeftWindow(true, true, true);
                    windowsService.ActivateRightWindow(null);
                    break;
                case PlayerActiveMenuType.Gun:
                    windowsService.ActivatePlayerInventoryAsLeftWindow(false, false, true);
                    windowsService.ActivateRightWindow(gunInventoryWindow);
                    break;
                case PlayerActiveMenuType.Potion:
                    windowsService.ActivatePlayerInventoryAsLeftWindow(false, true, false);
                    windowsService.ActivateRightWindow(potionInventoryWindow);
                    break;
                case PlayerActiveMenuType.QuickCraft:
                    break;
            }
        }
    }

    public enum PlayerActiveMenuType
    {
        None = 0,
        Gun,
        Potion,
        QuickCraft
    }
}