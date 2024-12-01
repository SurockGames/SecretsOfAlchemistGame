using System.Collections;
using UnityEngine;

namespace Assets.Code.Shooting
{
    public class CopyPosition : MonoBehaviour
    {
        public bool IsCopyingPosition = true;
        public bool CopyLocal = true;

        [SerializeField] private Transform copyTrs;

        private Transform trs;

        private void Awake()
        {
            trs = transform;
        }

        void LateUpdate()
        {
            if (!IsCopyingPosition) return;

            if (CopyLocal)
                trs.localPosition = copyTrs.localPosition;
            else
                trs.position = copyTrs.position;
        }
    }
}