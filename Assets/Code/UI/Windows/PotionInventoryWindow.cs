using System;
using UnityEngine;

namespace Assets.Code
{
    public class PotionInventoryWindow : InventoryWindow, ILeftWindow
    {
        [SerializeField] private SlotVisual potionSlot;

        public event Action OnInventoryUpdated;
        public Potion Potion => potionSlot.Slot.EquipedItem ? potionSlot.Slot.EquipedItem as Potion : null;
        public int AmountOfPatrons => potionSlot.Slot.Amount;

        public override void Initialize()
        {
            base.Initialize();

            InitializeSlot(potionSlot);
        }

        public override void UpdateInventory()
        {
            var potion = inventory.GetAllItems()[0];

            if (potion != null)
            {
                UpdateSlot(potionSlot, potion.Amount, potion.Item);
            }
            else
            {
                potionSlot.ClearToDefault();
            }

        }
    }
}