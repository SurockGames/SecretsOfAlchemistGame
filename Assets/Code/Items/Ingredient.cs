using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code
{
    // Granade 
    // Patron 
    // Potion

    // ITEM: 
    // Ingredient 
    //     Item variant 
    // Arsenal (granades and patrons)
    //     Item variant
    // Potions 
    //     Item variant 


    // Open Grind Window => 
    // Open Player Ingredient Window 
    // Scan all ingredient and check for Type Grinded
    // Sort Ingredients that Type Grinded are on top and other is grayed and uninteractable 
    // Mark slots that contains Grinded Ingredients as interactable
    // Display all interaction buttons available on that window: drag item, quick take all stack, back, return to game 
    // When player hover Interactable slot => show info pop-up about item; Highlight the Grind Item Slot (Craft window feature)
    // When player press button - "take all stack", on Interactable slot =>
    //      Delete item stack from inventory slot,
    //      add this stack to first empty Craft slot or swap stack with Item that is in Craft slot
    //          If take the stack then sort items in inventory again
    //      Add Grinded Type Item of moved Item stack to Output slot (grayed out) 
    //      Show "Craft" button
    // When player press button "Take amount" => activate slider with min 1 and max Item.Amount of that stack -- there will be 2 buttons "Take" and "Cancel"
    // When player press "Craft" button close to Grind Craft Output Slot => delete the stack of Items in Craft slot and add grinded stack to player's inventory 
    // If player press button "Take to Inventory" on Craft slot stack => move stack to player's inventory, sort inventory again

    public interface IIngredient { }

    [CreateAssetMenu(fileName = "Data", menuName = "Ingredients/Ingredient", order = 1)]
    public class Ingredient : Item, IIngredient
    {
        [field: SerializeField] public Dictionary<IngredientVariantTypes, IngredientVariant> cookedTypes { get; private set; }

        public override ItemTypes ItemType => ItemTypes.Ingredient;

        public bool HasCookedType(IngredientVariantTypes cookedType)
        {
            var entries = cookedTypes.Where(entry => entry.Key.HasFlag(cookedType));
            if (entries.Count() == 0) 
                return false;

            return true;
        }

        public IngredientVariant GetIngredientVariantOfType(IngredientVariantTypes type)
        {
            var entries = cookedTypes.Where(entry => entry.Key.HasFlag(type));

            return entries.ElementAt(0).Value;
        }

#if UNITY_EDITOR
        public bool RegisterCookedVariant(IngredientVariant ingredientVariant)
        {
            cookedTypes.TryGetValue(ingredientVariant.CookedType, out var ingredient);
            if (ingredient)
            {
                if (ingredient != ingredientVariant)
                {
                    Debug.LogException(new Exception($"There is another cooked variant of {ingredientVariant.CookedType}, that is {ingredient.Name}"));
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (cookedTypes.ContainsKey(ingredientVariant.CookedType))
                cookedTypes[ingredientVariant.CookedType] = ingredientVariant;
            else
                cookedTypes.Add(ingredientVariant.CookedType, ingredientVariant);

            return true;
        }
#endif
    }
}