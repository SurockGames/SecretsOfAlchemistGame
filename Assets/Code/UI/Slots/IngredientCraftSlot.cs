
using UnityEngine;

namespace Assets.Code
{
    public class IngredientCraftSlot : Slot
    {
        public IngredientVariantTypes cookedType;
        public override bool CanEquipItem(Item item)
        {
            if (item is ItemVariant)
            {
                Debug.Log("Ingredient craft slot can equip only original ingredients!");
                return false;
            }

            if (item is Ingredient)
            {
                var ingredient = item as Ingredient;

                if (equipedItem)
                    if (equipedItem != item)
                        return false;

                if (ingredient.HasCookedType(cookedType)) 
                    return true;
            }

            return false;
        }
    }
}