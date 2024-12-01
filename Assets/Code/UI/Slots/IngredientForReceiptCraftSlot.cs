
using UnityEngine;

namespace Assets.Code
{
    public class IngredientForReceiptCraftSlot : Slot
    {
        public IngredientVariantTypes cookedType;
        public override bool CanEquipItem(Item item)
        {
            // Another item is in this slot
            if (equipedItem)
                if (equipedItem != item)
                    return false;

            if (item is ItemVariant || item is Ingredient)
            {
                return true;
            }

            return false;
        }
    }
}