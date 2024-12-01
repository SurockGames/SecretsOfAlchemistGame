using Assets.Code;
using UnityEngine;

public class PotionSlotVisuals : MonoBehaviour
{
    public GameObject hintButtonDrink;
    public SlotVisual slot;
    public PotionInventory potionInventory;

    private void Start()
    {
        potionInventory.OnInventoryChanged += UpdateSlot;
    }

    private void UpdateSlot()
    {
        slot.SetIcon(potionInventory.PotionInSlot.Icon);
        slot.SetAmount(potionInventory.AmountOfPotionInSlot);

        if (potionInventory.PotionInSlot == null)
        {
            hintButtonDrink.SetActive(false);
        }
        else
        {
            hintButtonDrink.SetActive(true);
        }
    }
}
