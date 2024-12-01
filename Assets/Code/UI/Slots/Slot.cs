
using System;
using UnityEngine;

namespace Assets.Code
{
    public abstract class Slot : MonoBehaviour, IItemEquitable
    {
        public ItemTypes slotItemType;
        public Item EquipedItem => equipedItem;
        public int Amount => amount;

        protected Item equipedItem;
        private int amount;

        public virtual bool CanEquipItem(Item item)
        {
            if (equipedItem)
                if (equipedItem != item)
                    return false;

            return (slotItemType & item.ItemType) > 0;
        }

        public void EquipItem(Item item)
        {
            if (!CanEquipItem(item)) return;

            equipedItem = item;
        }

        public void SetAmount(int amount)
        {
            this.amount = amount;
        }

        public void ClearSlot()
        {
            equipedItem = null;
            amount = 0;
        }
    }

    public interface IItemEquitable
    {
        bool CanEquipItem(Item item);

        void EquipItem(Item item);
    }

    public interface IIngredientEquitable
    {
        bool CanEquipIngredient(IngredientVariantTypes compareType);
    }
}