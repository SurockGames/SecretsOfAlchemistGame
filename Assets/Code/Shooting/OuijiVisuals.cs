using UnityEngine;
using UnityEngine.Events;

namespace Assets.Code.Shooting
{
    public class OuijiVisuals : MonoBehaviour
    {
        public GameObject text;
        public Animator animator;
        protected static readonly int EquipedHash = Animator.StringToHash("Equiped");



        public UnityEvent OnEquip;

        public void GetOutOuiji()
        {
            text.SetActive(true);
            animator.SetBool(EquipedHash, true);
            OnEquip?.Invoke();
        }

        public void HideOuiji()
        {
            text.SetActive(false);
            animator.SetBool(EquipedHash, false);
            OnEquip?.Invoke();
        }
    }
}