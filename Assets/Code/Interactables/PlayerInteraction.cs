using Assets.Code;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private InteractableService interactableService;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private float interactDistance = 7;

    private Transform cameraTrs;

    private void Start()
    {
        cameraTrs = cameraMain.transform;
    }

    void Update()
    {
        if (!cameraTrs) return;

        if (Physics.Raycast(cameraTrs.position, cameraTrs.forward, out RaycastHit hit, interactDistance, targetMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (hit.collider.TryGetComponent(out InvisibleObject invisibleObject))
                {
                    if (game.PlayerStatsService.CanSeeInvisible)
                    {
                        interactableService.CanInteractWith(interactable);
                        return;
                    }
                }
                else
                {
                    interactableService.CanInteractWith(interactable);
                    return;
                }
            }
        }

        interactableService.NothingToInteract();
    }

    public bool TakeItem(Item item, int amount)
    {
        if (game.PlayerInventoryService.TryAddItemToInventory(item, amount))
        {
            game.GatherPopupService.CreateGatherPopup(item, amount);
            return true;
        }
        return false;
    }
}
