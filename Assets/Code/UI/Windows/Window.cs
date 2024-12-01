using Assets.Code.Items;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Code
{
    public interface IInventoryWindow
    {
        bool TryTransferItemToThisInventory(Item item, int amount);
        bool CheckForTransferItemOpportunity(Item item);
    }

    public abstract class Window : SerializedMonoBehaviour, IInventoryWindow
    {
        [SerializeField] protected WindowsService windowsService;
        [SerializeField] protected ItemInfoPopup itemInfoPopup;

        protected SlotVisual hoveredSlot;
        protected SlotVisual selectedSlot;

        [ShowInInspector, ReadOnly]
        protected bool isHoveredSlotCanTransferItem;
        protected bool isSelectedSlotCanTransferItem;

        protected bool isMoveStack;

        public abstract bool TryTransferItemToThisInventory(Item item, int amount);
        public abstract bool CheckForTransferItemOpportunity(Item item);
        public abstract void SetWindow(bool isActive);

        private void Update()
        {
            if (isHoveredSlotCanTransferItem)
            {
                if (Input.GetKey(windowsService.MoveWholeStackKey))
                {
                    isMoveStack = true;
                }
                else
                {
                    isMoveStack = false;
                }
            }

            if (isSelectedSlotCanTransferItem)
            {
                isMoveStack = false;
            }
        }

        public virtual void OnSlotHover(SlotVisual slotVisual)
        {
            itemInfoPopup.gameObject.SetActive(true);

            hoveredSlot = slotVisual;
            itemInfoPopup.SetInfo(slotVisual.name, slotVisual.description);

            CheckSlotIfTransferable();
        }

        protected virtual void CheckSlotIfTransferable()
        {
            if (hoveredSlot == null) return;
            if (!hoveredSlot.CanBeTransfered) return;

            if (windowsService.CanTransferItemToOtherWindow(hoveredSlot.Slot.EquipedItem, this))
            {
                isHoveredSlotCanTransferItem = true;
                windowsService.SetItemTransferHint(true);
            }
        }

        public virtual void OnSlotUnhover(SlotVisual slotVisual)
        {
            itemInfoPopup.gameObject.SetActive(false);

            hoveredSlot = null;
            isHoveredSlotCanTransferItem = false;
            windowsService.SetItemTransferHint(false);
        }

        public virtual void OnSlotClicked(SlotVisual slotVisual)
        {
            if (!isHoveredSlotCanTransferItem) return;

            if (isMoveStack && selectedSlot == null)
            {
                TransferItem(hoveredSlot, hoveredSlot.Slot.Amount);
            }
            else
            {
                selectedSlot = hoveredSlot;
                isSelectedSlotCanTransferItem = true;

                windowsService.ShowItemSelectAmountSlider(hoveredSlot.Slot.Amount, CancelSelection, ConfirmSelection);
            }
        }

        protected void CancelSelection()
        {
            selectedSlot = null;
            isSelectedSlotCanTransferItem = false;
        }

        protected void ConfirmSelection(int amount)
        {
            if (selectedSlot == null) return;

            TransferItem(selectedSlot, amount);
        }


        protected virtual void TransferItem(SlotVisual from, int amount)
        {
            if (from == null) return;

            if (from == hoveredSlot)
            {
                TryTransferItemFromSlot(hoveredSlot, amount);
            }
            else if (from == selectedSlot)
            {
                TryTransferItemFromSlot(selectedSlot, amount);
            }

            CancelSelection();

            if (hoveredSlot)
                OnSlotHover(hoveredSlot);
        }

        protected virtual bool TryTransferItemFromSlot(SlotVisual transferFromSlot, int amount, bool transferToPlayerInventory = false)
        {
            if (windowsService.TransferItemToOtherWindow(transferFromSlot.Slot.EquipedItem, amount, this))
            {
                if (transferFromSlot.Slot.Amount == amount)
                {
                    ClearSlot(transferFromSlot);
                }
                else
                {
                    RemoveItemAmountFromSlot(transferFromSlot, amount);
                }

                return true;
            }
            return false;
        }

        protected virtual void ClearSlot(SlotVisual slot)
        {
            slot.ClearToDefault();
        }

        protected virtual void RemoveItemAmountFromSlot(SlotVisual slot, int amount)
        {
            int amountLeft = slot.Slot.Amount - amount;
            slot.SetAmount(amountLeft);
        }
    }

    public interface ILeftWindow { }
    public interface IRightWindow { }

    [Serializable]
    public class ItemStack
    {
        public Item Item;
        public int Amount;

        public ItemStack(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }
    }

    public interface ICraftWindow
    {
        void Craft();
    }
}