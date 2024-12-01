using Assets.Code.Shooting;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Code
{
    public class Gun : MonoBehaviour, IInteractable
    {
        public PlayerHandsService playerHandsService;
        public GunSO gunData;
        public Patron patronWithGun;
        public int ammoAmount = 6;

        public KeyCode InteractKey => KeyCode.E;

        [SerializeField] private string Name;

        public override string ToString()
        {
            return Name;
        }

        [Button]
        public virtual void Interact()
        {
            playerHandsService.TryCollectGun(gunData, new ItemStack(patronWithGun, ammoAmount));
        }
    }
}