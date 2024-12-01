using System.Collections;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class CopyRotation : MonoBehaviour
    {
        public bool IsCopyingRotation = true;
        public bool CopyLocal = true;

        [SerializeField] private Transform copyTrs;

        private Transform trs;

        private void Awake()
        {
            trs = transform;
        }

        void LateUpdate()
        {
            if (!IsCopyingRotation) return;

            if (CopyLocal)
                trs.localRotation = copyTrs.localRotation;
            else
                trs.rotation = copyTrs.rotation;
        }
    }
}