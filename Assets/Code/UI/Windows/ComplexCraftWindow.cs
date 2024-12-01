using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class ComplexCraftWindow : Window, ICraftWindow, IRightWindow
    {
        [SerializeField] private GameObject craftWindowGroup;
        [SerializeField] private Button craftButton;
        [SerializeField] protected List<Receipt> receipts;

        public event Action OnCraftSlotSet;
        public event Action OnCraft;

        public List<SlotVisual> CraftingSlots;

        public SlotVisual OutputSlot;

        private int slotsAmount;

        [ShowInInspector, ReadOnly]
        private Receipt currentActiveReceipt;


        private void Awake()
        {
            OutputSlot.Initialize(slotsAmount);
            slotsAmount++;

            foreach (SlotVisual slot in CraftingSlots)
            {
                slot.Initialize(slotsAmount);
                slotsAmount++;

                slot.OnSlotHovered += OnSlotHover;
                slot.OnSlotUnhovered += OnSlotUnhover;
                slot.OnSlotClicked += OnSlotClicked;
            }

            OnCraftSlotSet += OnCraftSlotEquiped;

            craftButton.onClick.AddListener(Craft);

            OutputSlot.OnSlotHovered += OnSlotHover;
            OutputSlot.OnSlotUnhovered += OnSlotUnhover;
        }

        private void OnDestroy()
        {
            foreach (SlotVisual slot in CraftingSlots)
            {
                slot.Initialize(slotsAmount);
                slotsAmount++;

                slot.OnSlotHovered -= OnSlotHover;
                slot.OnSlotUnhovered -= OnSlotUnhover;
                slot.OnSlotClicked -= OnSlotClicked;
            }

            OnCraftSlotSet -= OnCraftSlotEquiped;

            craftButton.onClick.RemoveListener(Craft);

            OutputSlot.OnSlotHovered -= OnSlotHover;
            OutputSlot.OnSlotUnhovered -= OnSlotUnhover;
        }

        private void OnCraftSlotEquiped()
        {
            CraftButtonSetEnabled(true);

            // FIND RECEIPT 

            TrySetOutputSlot();
        }

        public bool TryCraftItem(List<ItemStack> itemsToCook, out ItemStack craftedItem)
        {
            craftedItem = null;

            var maxCoincededItems = 0;
            ItemStack itemFromReceipt;

            foreach (var receipt in receipts)
            {
                itemFromReceipt = receipt.TryGetItem(itemsToCook, out int coincidedItemsFromReceiptAmount, out int amountCanBeCooked);

                if (coincidedItemsFromReceiptAmount > maxCoincededItems)
                {
                    craftedItem = itemFromReceipt;
                    //craftedItem.Amount = amountCanBeCooked;

                    currentActiveReceipt = receipt;
                    maxCoincededItems = coincidedItemsFromReceiptAmount;
                }
            }

            if (craftedItem != null)
                return true;

            currentActiveReceipt = null;
            return false;
        }

        protected virtual ItemStack FindCraftItemFromReceipts(List<SlotVisual> cookingSlots)
        {
            var itemsToCook = new List<ItemStack>();

            foreach (var slot in cookingSlots)
            {
                if (slot.Slot.EquipedItem != null)
                    itemsToCook.Add(new ItemStack(slot.Slot.EquipedItem, slot.Slot.Amount));
            }

            if (itemsToCook.Count > 0)
            {
                var stack = TryCraftItem(itemsToCook, out ItemStack outputItemStack);
                return outputItemStack;
            }

            return null;
            //OutputSlot.SetItem()
        }

        protected virtual bool TrySetOutputSlot()
        {
            var itemStack = FindCraftItemFromReceipts(CraftingSlots);

            if (itemStack != null)
            {
                OutputSlot.SetItem(itemStack.Item);
                OutputSlot.SetAmount(itemStack.Amount);
                return true;
            }
            else
            {
                OutputSlot.ClearToDefault();
                return false;
            }
        }

        private void CraftButtonSetEnabled(bool enabled)
        {
            craftButton.gameObject.SetActive(enabled);
        }

        public override void SetWindow(bool isActive)
        {
            if (!isActive)
            {
                foreach (var slot in CraftingSlots)
                {
                    TryTransferItemFromSlot(slot, slot.Slot.Amount);
                }
            }

            craftWindowGroup.SetActive(isActive);
        }

        private void EvaluateAmountFromReceipt()
        {
            //currentActiveReceipt.EvaluateAmount();
        }

        public virtual void Craft()
        {

            if (TryTransferItemFromSlot(OutputSlot, OutputSlot.Slot.Amount))
            {
                foreach (var slot in CraftingSlots)
                {
                    slot.ClearToDefault();
                }
                OutputSlot.ClearToDefault();
            }
        }

        protected override void ClearSlot(SlotVisual slot)
        {
            base.ClearSlot(slot);

            if (OutputSlot.Slot.EquipedItem != null)
                TrySetOutputSlot();

            var isAllInputSlotsEmpty = true;
            foreach (var craftSlot in CraftingSlots)
            {
                if (craftSlot.Slot.EquipedItem != null)
                {
                    isAllInputSlotsEmpty = false;
                    break;
                }
            }

            if (isAllInputSlotsEmpty)
            {
                CraftButtonSetEnabled(false);

                OutputSlot.ClearToDefault();
            }
        }

        public override bool TryTransferItemToThisInventory(Item item, int amount)
        {
            foreach (var slot in CraftingSlots)
            {
                if (slot.Slot.CanEquipItem(item))
                {
                    if (slot.Slot.EquipedItem == null)
                    {
                        slot.SetItem(item);
                        slot.SetAmount(amount);
                    }
                    else
                    {
                        slot.SetAmount(slot.Slot.Amount + amount);
                    }

                    OnCraftSlotSet?.Invoke();
                    return true;
                }
            }
            return false;
        }

        protected override void RemoveItemAmountFromSlot(SlotVisual slot, int amount)
        {
            base.RemoveItemAmountFromSlot(slot, amount);

            OnCraftSlotSet?.Invoke();
        }

        public override bool CheckForTransferItemOpportunity(Item item)
        {
            foreach (var slot in CraftingSlots)
            {
                if (slot.Slot.CanEquipItem(item))
                {
                    return true;
                }
            }
            return false;
        }

        protected override void CheckSlotIfTransferable()
        {
            if (hoveredSlot == OutputSlot)
                return;

            base.CheckSlotIfTransferable();
        }

        public override void OnSlotClicked(SlotVisual slotVisual)
        {
            if (slotVisual == OutputSlot) return;

            base.OnSlotClicked(slotVisual);
        }
    }
}