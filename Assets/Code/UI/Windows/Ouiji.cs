using Assets.Code.Shooting;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Code
{
    public class Ouiji : MonoBehaviour, IInteractable
    {
        public PlayerHandsService playerHandsService;

        public KeyCode InteractKey => KeyCode.E;

        [SerializeField] private string Name;

        public override string ToString()
        {
            return Name;
        }

        [Button]
        public virtual void Interact()
        {
            playerHandsService.CollectOuiji();
        }
    }
}