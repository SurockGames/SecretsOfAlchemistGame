using Assets.Code;
using System;
using UnityEngine;

public class InteractableService : MonoBehaviour
{
    [SerializeField] private GameObject uiInteractHint;

    private IInteractable activeInteractable;
    private bool canInteract = true;

    public void CanInteractWith(IInteractable interactable)
    {
        if (!canInteract) return;
        if (interactable == null) return;

        if (activeInteractable == interactable) return;

        activeInteractable = interactable;
        ShowInteractKey(interactable);
    }

    public void LockInteraction()
    {
        canInteract = false;
        NothingToInteract();
    }

    public void UnlockInteraction()
    {
        canInteract = true;
    }

    public void NothingToInteract()
    {
        activeInteractable = null;

        HideInteractKey();
    }

    private void Update()
    {
        if (activeInteractable == null) return;

        if (Input.GetKeyDown(activeInteractable.InteractKey))
        {
            activeInteractable.Interact();
        }
    }

    private void HideInteractKey()
    {
        uiInteractHint.SetActive(false);
    }
    public void ShowInteractKey(IInteractable interactable)
    {
        uiInteractHint.SetActive(true);
    }
}
