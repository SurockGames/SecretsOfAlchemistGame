using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class GrindingCraftWindow : CraftWindow
    {
        [SerializeField] private IngredientVariantTypes ingredientVariantType;

        protected override Item FindCraftItemVariant(Item originalItem)
        {
            var originalIngredient = originalItem as Ingredient;

            if (originalIngredient)
            {
                return originalIngredient.GetIngredientVariantOfType(ingredientVariantType);
            }

            return originalItem;
            //OutputSlot.SetItem()
        }
    }

    public class PotionCraftWindow : CraftWindow
    {
        [SerializeField] private IngredientVariantTypes ingredientVariantType;

        protected override Item FindCraftItemVariant(Item originalItem)
        {
            var originalIngredient = originalItem as Ingredient;

            if (originalIngredient)
            {
                return originalIngredient.GetIngredientVariantOfType(ingredientVariantType);
            }

            return originalItem;
            //OutputSlot.SetItem()
        }
    }
}