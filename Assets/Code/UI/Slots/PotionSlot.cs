namespace Assets.Code
{
    public class PotionSlot : Slot
    {
        public override bool CanEquipItem(Item item)
        {
            if (item is Potion || item is PotionVariant)
                return true;

            return false;
        }
    }
}