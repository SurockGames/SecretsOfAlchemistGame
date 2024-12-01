using Assets.Code;
using Assets.Code.Infrastructure;
using Assets.Code.Shooting;
using KinematicCharacterController.Examples;
using UnityEngine;

public class GameplayState : BaseGameState
{
    protected readonly WindowsService windowsService;

    public GameplayState(Game game,
                        PlayerService playerService,
                        GameTimeService gameTimeService,
                        PlayerGunShootingService gunShootingService,
                        InteractableService interactableService,
                        WindowsService windowsService)
        : base(game, playerService, gameTimeService, gunShootingService, interactableService)
    {
        this.windowsService = windowsService;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter gameplay state");
        playerService.UnlockCharacter();
        gameTimeService.UnfreezeTime();
        gunShootingService.UnlockShooting();
        interactableService.UnlockInteraction();
        windowsService.CloseAllWindows();
    }

    public override void OnExit()
    {
        //playerService.LockCharacter();
        //gameTimeService.FreezeTime();
        //gunShootingService.LockShooting();
        //interactableService.LockInteraction();
    }
}
