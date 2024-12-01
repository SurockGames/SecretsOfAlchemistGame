using KinematicCharacterController.Examples;
using UnityEngine;

namespace Assets.Code
{
    public class OuijiTrigger : MonoBehaviour
    {
        public Game game;
        public string Message;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out KinematicCharacterController.Examples.CharacterController character))
            {
                game.BoardTalkingService.ActivateTrigger(Message);
            }
        }
    }
}