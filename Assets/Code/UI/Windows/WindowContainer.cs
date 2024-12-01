using Assets.Code.Shooting;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Code
{
    public abstract class WindowContainer : MonoBehaviour, IInteractable
    {
        public WindowsService windowsService;
        public KeyCode InteractKey => KeyCode.E;

        [SerializeField] private string Name;

        [SerializeField] private Transform cameraMovePosition;
        public WindowGroup windowGroup;


        public override string ToString()
        {
            return Name;
        }

        [Button]
        public virtual void Interact()
        {
            if (windowGroup != null)
            {
                if (windowGroup.TryActivateWindowContainer(this))
                {
                    windowsService.ActivateTabsGroup(windowGroup);
                }

            }
        }

        public abstract void Activate();

        // Stop player movement 
    }
}