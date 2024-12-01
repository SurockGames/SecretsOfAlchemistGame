using System;

namespace Assets.Code
{
    public enum ItemTypes
    {
        None,
        Ingredient,
        Potion,
        Ammunition
    }

    [Flags]
    public enum IngredientVariantTypes
    {
        None = 0,
        Grinded = 1<<1,
        Boiled = 1<<2,
        Steamed = 1<<3,

        All = ~0
    }

    public enum PotionTypes
    {
        None,
    }

    [Flags]
    public enum AmmunitionTypes
    {
        None = 0,
        Granade = 1 << 1,
        Patrons = 1 << 2,

        All = ~0
    }
}