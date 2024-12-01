using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class CraftWindow : Window, ICraftWindow, IRightWindow
    {
        [SerializeField] private GameObject craftWindowGroup;
        [SerializeField] private Button craftButton;

        public event Action OnCraftSlotSet;
        public event Action OnCraft;

        public SlotVisual CraftingSlot;

        public SlotVisual OutputSlot;

        private void Awake()
        {
            CraftingSlot.Initialize(0);
            OutputSlot.Initialize(1);

            OnCraftSlotSet += OnCraftSlotEquiped;
        }

        private void OnEnable()
        {
            CraftingSlot.OnSlotHovered += OnSlotHover;
            OutputSlot.OnSlotHovered += OnSlotHover;

            craftButton.onClick.AddListener(Craft);

            CraftingSlot.OnSlotUnhovered += OnSlotUnhover;
            OutputSlot.OnSlotUnhovered += OnSlotUnhover;

            CraftingSlot.OnSlotClicked += OnSlotClicked;
        }

        private void OnDisable()
        {
            CraftingSlot.OnSlotHovered -= OnSlotHover;
            OutputSlot.OnSlotHovered -= OnSlotHover;

            craftButton.onClick.RemoveListener(Craft);

            CraftingSlot.OnSlotUnhovered -= OnSlotUnhover;
            OutputSlot.OnSlotUnhovered -= OnSlotUnhover;

            CraftingSlot.OnSlotClicked -= OnSlotClicked;
        }

        private void OnDestroy()
        {
            OnCraftSlotSet -= OnCraftSlotEquiped;
        }

        private void OnCraftSlotEquiped()
        {
            CraftButtonSetEnabled(true);

            // FIND RECEIPT 

            TrySetOutputSlot();
        }

        protected virtual Item FindCraftItemVariant(Item originalItem)
        {
            return originalItem;
            //OutputSlot.SetItem()
        }

        protected virtual bool TrySetOutputSlot()
        {
            var item = FindCraftItemVariant(CraftingSlot.Slot.EquipedItem);
            OutputSlot.SetItem(item);
            OutputSlot.SetAmount(CraftingSlot.Slot.Amount);
            return true;
        }

        private void CraftButtonSetEnabled(bool enabled)
        {
            craftButton.gameObject.SetActive(enabled);
        }

        public override void SetWindow(bool isActive)
        {
            if (!isActive)
            {
                TryTransferItemFromSlot(CraftingSlot, CraftingSlot.Slot.Amount);
            }

            craftWindowGroup.SetActive(isActive);
        }

        public virtual void Craft()
        {
            if (TryTransferItemFromSlot(OutputSlot, OutputSlot.Slot.Amount))
            {
                CraftingSlot.ClearToDefault();
                OutputSlot.ClearToDefault();
            }
        }

        protected override void ClearSlot(SlotVisual slot)
        {
            base.ClearSlot(slot);

            CraftButtonSetEnabled(false);

            OutputSlot.ClearToDefault();
            // OUTPUT Slot Clear
        }

        public override bool TryTransferItemToThisInventory(Item item, int amount)
        {
            if (CraftingSlot.Slot.CanEquipItem(item))
            {
                if (CraftingSlot.Slot.EquipedItem == null)
                {
                    CraftingSlot.SetItem(item);
                    CraftingSlot.SetAmount(amount);
                }
                else
                {
                    CraftingSlot.SetAmount(CraftingSlot.Slot.Amount + amount);
                }

                OnCraftSlotSet?.Invoke();
                return true;
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
            if (CraftingSlot.Slot.CanEquipItem(item))
            {
                return true;
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