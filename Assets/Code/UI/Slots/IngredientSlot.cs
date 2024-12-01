namespace Assets.Code
{
    public class IngredientSlot : Slot
    {
        public override bool CanEquipItem(Item item)
        {
            if (item is Ingredient || item is IngredientVariant)
                return true;
            
            return false;
        }
    }
}