using UnityEngine;

namespace Assets.Code
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        public KeyCode InteractKey => KeyCode.E;

        public abstract void Interact();
    }

    public interface IInteractable
    {
        public KeyCode InteractKey { get; }
        public void Interact();
    }
}