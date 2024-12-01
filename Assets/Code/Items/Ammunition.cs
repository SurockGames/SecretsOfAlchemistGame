using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "Data", menuName = "Ammunitions/Ammunition", order = 1)]
    public class Ammunition : Item
    {
        [field: SerializeField] public Dictionary<AmmunitionTypes, AmmunitionVariant> AmmunitionTypes { get; private set; }

        public override ItemTypes ItemType => ItemTypes.Ammunition;

        public bool HasAmmunitionType(AmmunitionTypes ammunitionType)
        {
            if (!AmmunitionTypes.ContainsKey(ammunitionType))
                return false;

            if (AmmunitionTypes[ammunitionType] == null)
                return false;

            return AmmunitionTypes.ContainsKey(ammunitionType);
        }
#if UNITY_EDITOR
        public bool RegisterAmmunitionVariant(AmmunitionVariant ammunitionVariant)
        {
            AmmunitionTypes.TryGetValue(ammunitionVariant.AmmunitionType, out var ammunition);
            if (ammunition)
            {
                if (ammunition != ammunitionVariant)
                {
                    Debug.LogException(new Exception($"There is another ammunition variant of {ammunitionVariant.AmmunitionType}, that is {ammunition.Name}"));
                    return false;
                }
                else
                {
                    return true;
                }
            }

            if (AmmunitionTypes.ContainsKey(ammunitionVariant.AmmunitionType))
                AmmunitionTypes[ammunitionVariant.AmmunitionType] = ammunitionVariant;
            else
                AmmunitionTypes.Add(ammunitionVariant.AmmunitionType, ammunitionVariant);

            return true;
        }
#endif
    }
}