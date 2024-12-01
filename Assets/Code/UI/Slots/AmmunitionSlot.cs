namespace Assets.Code
{
    public class AmmunitionSlot : Slot
    {
        public override bool CanEquipItem(Item item)
        {
            if (item is Ammunition || item is AmmunitionVariant || item is Patron)
                return true;

            return false;
        }
    }
}