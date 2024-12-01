using System.Collections;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class ChildPosition : MonoBehaviour
    {
        public bool IsChild = true;
        public bool CopyLocal = true;
        [SerializeField] private Transform copyTrs;

        [SerializeField] private Vector3 offset;
        private Transform trs;

        private void Awake()
        {
            trs = transform;
            //offset = trs.localPosition;
        }

        void LateUpdate()
        {
            if (!IsChild) return;

            if (CopyLocal)
                trs.localPosition = copyTrs.localPosition + offset;
            else
                trs.position = copyTrs.position + offset;
        }
    }
}