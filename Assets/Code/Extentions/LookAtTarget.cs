using System.Collections;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class LookAtTarget : MonoBehaviour
    {
        public bool IsLookingAtTarget = true;
        [SerializeField] private Transform lookAt;

        private Transform trs;

        private void Awake()
        {
            trs = transform;
        }

        void LateUpdate()
        {
            if (!IsLookingAtTarget) return;

            trs.LookAt(lookAt);    
        }
    }
}