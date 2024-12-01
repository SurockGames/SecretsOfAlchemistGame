using System.Collections;
using UnityEngine;

namespace Assets.Code.Enemy
{
    public class PlayerTargetPosition : MonoBehaviour
    {
        [SerializeField] private Transform playerTrs;
        [SerializeField] private float smoothSpeed = 3;

        private Vector3 prevPlayerPosition;
        private Vector3 velocity;

        private void Update()
        {
            playerTrs.position = Vector3.SmoothDamp(playerTrs.position, playerTrs.position - prevPlayerPosition, ref velocity, smoothSpeed * Time.deltaTime);
        }
    }
}