using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "Data", menuName = "Potions/Potion", order = 1)]
    public class Potion : Item
    {
        [field: SerializeField] public Dictionary<PotionTypes, PotionVariant> potionTypes { get; private set; }

        public PotionEffectTypes EffectTypes;
        public float timeWorking;
        public int healthRegenAmount;

        public override ItemTypes ItemType => ItemTypes.Potion;

        public bool HasPotionType(PotionTypes potionType)
        {
            if (!potionTypes.ContainsKey(potionType))
                return false;

            if (potionTypes[potionType] == null)
                return false;

            return potionTypes.ContainsKey(potionType);
        }
#if UNITY_EDITOR
        public bool RegisterPotionVariant(PotionVariant potionVariant)
        {
            potionTypes.TryGetValue(potionVariant.PotionType, out var ingredient);
            if (ingredient)
            {
                if (ingredient != potionVariant)
                {
                    Debug.LogException(new Exception($"There is another potion variant of {potionVariant.PotionType}, that is {ingredient.Name}"));
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (potionTypes.ContainsKey(potionVariant.PotionType))
                potionTypes[potionVariant.PotionType] = potionVariant;
            else
                potionTypes.Add(potionVariant.PotionType, potionVariant);

            return true;
        }
#endif
    }
}