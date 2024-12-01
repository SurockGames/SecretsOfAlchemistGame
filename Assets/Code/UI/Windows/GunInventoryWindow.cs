using System;
using UnityEngine;

namespace Assets.Code
{
    public class GunInventoryWindow : InventoryWindow, ILeftWindow
    {
        [SerializeField] private SlotVisual patronSlot;

        public event Action OnInventoryUpdated;
        public Patron Patron => patronSlot.Slot.EquipedItem ? patronSlot.Slot.EquipedItem as Patron : null;
        public int AmountOfPatrons => patronSlot.Slot.Amount;

        public override void Initialize()
        {
            base.Initialize();

            InitializeSlot(patronSlot);
        }

        public override void UpdateInventory()
        {
            var patron = inventory.GetAllItems()[0];

            if (patron != null)
            {
                UpdateSlot(patronSlot, patron.Amount, patron.Item);
            }
            else
            {
                patronSlot.ClearToDefault();
            }
            
        }
    }
}