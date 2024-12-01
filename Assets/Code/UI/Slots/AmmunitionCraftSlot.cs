
using UnityEngine;

namespace Assets.Code
{
    public class AmmunitionCraftSlot : Slot
    {
        public AmmunitionTypes ammunitionType;
        public override bool CanEquipItem(Item item)
        {
            if (item is ItemVariant)
            {
                Debug.LogError("Ammunition craft slot can equip only original ammunition!");
                return false;
            }

            if (item is Ammunition)
            {
                var ingredient = item as Ammunition;

                if (ingredient.HasAmmunitionType(ammunitionType))
                    return true;
            }

            return false;
        }
    }
}