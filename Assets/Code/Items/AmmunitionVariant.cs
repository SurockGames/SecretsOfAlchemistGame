using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "Data", menuName = "Ammunitions/AmmunitionVariant", order = 2)]
    public class AmmunitionVariant : ItemVariant, IIngredient
    {
        public Ammunition OriginalItemVersion => (Ammunition)originalItemVersion;
        [field: SerializeField] public AmmunitionTypes AmmunitionType { get; private set; }

        public override ItemTypes ItemType => ItemTypes.Ammunition;
        [SerializeField] protected override Item originalItemVersion { get; set; }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!originalItemVersion) return;

            if (!(originalItemVersion is Ammunition))
            {
                Debug.LogError($"{originalItemVersion.Name} is not an Ammunition");
                originalItemVersion = null;
                return;
            }

            if (!OriginalItemVersion.RegisterAmmunitionVariant(this))
                originalItemVersion = null;
        }
#endif
    }
}