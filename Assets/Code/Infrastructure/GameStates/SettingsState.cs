using Assets.Code;
using Assets.Code.Infrastructure;
using Assets.Code.Shooting;
using KinematicCharacterController.Examples;

public class SettingsState : BaseGameState
{
    public SettingsState(Game game,
                        PlayerService playerService,
                        GameTimeService gameTimeService,
                        PlayerGunShootingService gunShootingService,
                        InteractableService interactableService)
        : base(game, playerService, gameTimeService, gunShootingService, interactableService)
    { }

    public override void OnEnter()
    {
        playerService.LockCharacter();
        gameTimeService.FreezeTime();
        gunShootingService.LockShooting();
        interactableService.LockInteraction();
    }

    public override void OnExit()
    {
        //playerService.LockCharacter();
        //gameTimeService.FreezeTime();
        //gunShootingService.LockShooting();
        //interactableService.LockInteraction();
    }
}