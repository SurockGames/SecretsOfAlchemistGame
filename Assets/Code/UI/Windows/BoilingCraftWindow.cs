using UnityEngine;

namespace Assets.Code
{
    public class BoilingCraftWindow : CraftWindow
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