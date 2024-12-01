namespace Assets.Code
{
    public class PatronSlot : Slot
    {
        public override bool CanEquipItem(Item item)
        {
            if (item is Patron)
                return true;

            return false;
        }
    }
}