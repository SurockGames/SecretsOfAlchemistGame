using Assets.Code.Shooting;
using TMPro;
using UnityEngine;

namespace Assets.Code
{
    public class PatronActiveSlotUI : MonoBehaviour
    {
        [SerializeField] private GunInventoryWindow gunInventoryWindow;
        [SerializeField] private PlayerGunShootingService playerGunShootingService;
        [SerializeField] private TMP_Text patronsAmountLoadedInPistol;
        [SerializeField] private TMP_Text patronsOfTypeInInventory;

        public SlotVisual SlotInPistol;
        public SlotVisual SlotInInventory;

        private void OnEnable()
        {
            gunInventoryWindow.OnInventoryUpdated += UpdateSlot;
            playerGunShootingService.OnPatronAmountChange += UpdateSlot;
        }

        private void OnDisable()
        {
            gunInventoryWindow.OnInventoryUpdated -= UpdateSlot;
            playerGunShootingService.OnPatronAmountChange -= UpdateSlot;
        }

        private void UpdateSlot()
        {
            patronsOfTypeInInventory.text = gunInventoryWindow.AmountOfPatrons.ToString();
            patronsAmountLoadedInPistol.text = playerGunShootingService.LoadedPatronsAmount.ToString();
            SlotInPistol.SetIcon(playerGunShootingService.ActivePatronItem.Icon);
        }
    }
}