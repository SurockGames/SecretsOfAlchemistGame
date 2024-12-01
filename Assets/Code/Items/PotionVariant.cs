using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "Data", menuName = "Potions/PotionVariant", order = 2)]
    public class PotionVariant : ItemVariant
    {
        public Potion OriginalItemVersion => (Potion)originalItemVersion;
        [field: SerializeField] public PotionTypes PotionType { get; private set; }

        public override ItemTypes ItemType => ItemTypes.Potion;
        [SerializeField] protected override Item originalItemVersion { get; set; }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!originalItemVersion) return;

            if (!(originalItemVersion is Potion))
            {
                Debug.LogError($"{originalItemVersion.Name} is not an Potion");
                originalItemVersion = null;
                return;
            }

            if (!OriginalItemVersion.RegisterPotionVariant(this))
                originalItemVersion = null;
        }
#endif
    }
}