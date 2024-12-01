using Assets.Code.Infrastructure;
using Assets.Code.Shooting;
using KinematicCharacterController.Examples;
using UnityEngine;

public class UIWindowState : BaseGameState
{
    public UIWindowState(Game game,
                        PlayerService playerService, 
                        GameTimeService gameTimeService, 
                        PlayerGunShootingService gunShootingService,
                        InteractableService interactableService) 
        : base(game, playerService, gameTimeService, gunShootingService, interactableService) 
    { }

    public override void OnEnter()
    {
        Debug.Log("Enter ui state");
        playerService.LockCharacter();
        gameTimeService.FreezeTime();
        gunShootingService.LockShooting();
        interactableService.LockInteraction();
    }

    public override void OnExit()
    {
        //playerService.UnlockCharacter();
        gameTimeService.UnfreezeTime();
        gunShootingService.UnlockShooting();
        interactableService.UnlockInteraction();
    }
}
