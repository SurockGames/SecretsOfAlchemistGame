using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class InventoryWindow : Window, ILeftWindow
    {
        [SerializeField] private GameObject inventoryGroup;

        [SerializeField] protected IInventoryService inventory;

        private int slotsAmount;

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
        }

        private void Start()
        {
            inventory.OnInventoryChanged += UpdateInventory;
        }

        private void OnDisable()
        {
            inventory.OnInventoryChanged -= UpdateInventory;
        }

        public virtual void Initialize()
        {
            slotsAmount = 0;
            
            // Initializi slots
        }

        protected void InitializeSlot(SlotVisual slot)
        {
            slot.OnSlotHovered += OnSlotHover;
            slot.OnSlotUnhovered += OnSlotUnhover;
            slot.OnSlotClicked += OnSlotClicked;

            slot.Initialize(slotsAmount);
            slotsAmount++;
        }

        public override void SetWindow(bool isActive)
        {
            inventoryGroup.SetActive(isActive);
        }

        public virtual void UpdateInventory()
        {
            // Get items from inventory
            
            // Update slots with items

            // Clear empty slots
        }

        protected void UpdateSlot(SlotVisual slot, int amount, Item item)
        {
            slot.SetAmount(amount);
            slot.SetItem(item);
        }

        protected override void TransferItem(SlotVisual from, int amount)
        {
            base.TransferItem(from, amount);

            UpdateInventory();
        }

        protected override void ClearSlot(SlotVisual slot)
        {
            inventory.RemoveItemStackFromInventory(slot.Slot.EquipedItem);

            base.ClearSlot(slot);
        }

        protected override void RemoveItemAmountFromSlot(SlotVisual slot, int amount)
        {
            base.RemoveItemAmountFromSlot(slot, amount);

            inventory.RemoveItemFromInventory(slot.Slot.EquipedItem, amount);
        }

        public override bool TryTransferItemToThisInventory(Item item, int amount)
        {
            if (inventory.TryAddItemToInventory(item, amount))
                return true;
            else
                return false;
        }

        public override bool CheckForTransferItemOpportunity(Item item)
        {
            return true;
        }
    }
}