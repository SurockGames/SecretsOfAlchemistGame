using KinematicCharacterController.Examples;
using UnityEngine;

public class PoisonFog : MonoBehaviour
{
    public Game game;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KinematicCharacterController.Examples.CharacterController playerStatsService))
        {
            game.PlayerStatsService.InPoison();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out KinematicCharacterController.Examples.CharacterController playerStatsService))
        {
            game.PlayerStatsService.OutFromPoison();
        }
    }
}