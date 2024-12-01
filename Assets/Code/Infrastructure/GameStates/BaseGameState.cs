using Assets.Code;
using Assets.Code.Infrastructure;
using Assets.Code.Shooting;
using KinematicCharacterController.Examples;

public abstract class BaseGameState : IState
{
    protected readonly Game game;
    protected readonly PlayerService playerService;
    protected readonly GameTimeService gameTimeService;
    protected readonly PlayerGunShootingService gunShootingService;
    protected readonly InteractableService interactableService;

    public BaseGameState(Game game, PlayerService playerService,
                        GameTimeService gameTimeService,
                        PlayerGunShootingService gunShootingService,
                        InteractableService interactableService)
    {
        this.playerService = playerService;
        this.gameTimeService = gameTimeService;
        this.gunShootingService = gunShootingService;
        this.interactableService = interactableService;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void OnExit()
    {

    }
}
