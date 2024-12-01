using Assets.Code;
using UnityEngine;

public class InvisibleObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Game game;
    [SerializeField] private Item item;
    [SerializeField] private int amount;


    public KeyCode InteractKey => KeyCode.E;

    public void Interact()
    {
        game.PlayerInteraction.TakeItem(item, amount);
    }
}
