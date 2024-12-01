
using UnityEngine;

namespace Assets.Code
{
    public class PotionCraftSlot : Slot
    {
        public PotionTypes potionType;
        public override bool CanEquipItem(Item item)
        {
            if (item is ItemVariant)
            {
                Debug.LogError("Potion craft slot can equip only original potions!");
                return false;
            }

            if (item is Potion)
            {
                var potion = item as Potion;

                if (potion.HasPotionType(potionType))
                    return true;
            }

            return false;
        }
    }
}