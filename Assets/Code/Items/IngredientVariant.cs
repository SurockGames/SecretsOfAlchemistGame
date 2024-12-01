using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(fileName = "Data", menuName = "Ingredients/IngredientVariant", order = 2)]
    public class IngredientVariant : ItemVariant, IIngredient
    {
        public Ingredient OriginalItemVersion => (Ingredient)originalItemVersion;
        [field: SerializeField] public IngredientVariantTypes CookedType { get; private set; }

        public override ItemTypes ItemType => ItemTypes.Ingredient;
        [SerializeField] protected override Item originalItemVersion { get; set; }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!originalItemVersion) return;

            if (!(originalItemVersion is Ingredient))
            {
                Debug.LogError($"{originalItemVersion.Name} is not an Ingredient");
                originalItemVersion = null;
                return;
            }

            if (!OriginalItemVersion.RegisterCookedVariant(this))
                originalItemVersion = null;
        }
#endif
    }
}