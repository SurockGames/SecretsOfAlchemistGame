using Assets.Code;
using Assets.Code.Infrastructure;
using Assets.Code.Items;
using Assets.Code.Shooting;
using KinematicCharacterController.Examples;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private PotionInventory potionInventory;
    [SerializeField] private PlayerInteraction playerInteraction;
    [SerializeField] private BoardTalkingService boardTalkingService;
    [SerializeField] private PlayerStatsService playerStatsService;
    [SerializeField] private PlayerService playerService;
    [SerializeField] private PlayerInventoryService playerInventoryService;
    [SerializeField] private WindowsService windowsService;
    [SerializeField] private InteractableService interactableService;
    [SerializeField] private GatherPopupService gatherPopupService;
    [SerializeField] private PlayerGunShootingService gunShootingService;
    [SerializeField] private GameTimeService gameTimeService;
    [SerializeField] private EnemyTargetService enemyTargetService;

    [SerializeField] private PlayerInventoryWindowContainer playerInventoryWindowContainer;
    
    private GameStateMachine gameStateMachine;
    private bool isUIWindowOpen;
    private bool isSettingsOpen;

    public PotionInventory PotionInventory => potionInventory;
    public PlayerInteraction PlayerInteraction => playerInteraction;
    public BoardTalkingService BoardTalkingService => boardTalkingService;
    public PlayerStatsService PlayerStatsService => playerStatsService;
    public PlayerService PlayerService => playerService; 
    public PlayerInventoryService PlayerInventoryService => playerInventoryService;
    public WindowsService WindowsService => windowsService; 
    public InteractableService InteractableService => interactableService; 
    public GatherPopupService GatherPopupService => gatherPopupService; 
    public PlayerGunShootingService GunShootingService => gunShootingService; 
    public GameTimeService GameTimeService => gameTimeService; 
    public EnemyTargetService EnemyTargetService => enemyTargetService;

    private void Awake()
    {
        gameStateMachine = new GameStateMachine();
        gameTimeService = new GameTimeService();

        // Declare states
        var gameplayState = new GameplayState(this, PlayerService, GameTimeService, GunShootingService, InteractableService, WindowsService);
        var uiWindowState = new UIWindowState(this, PlayerService, GameTimeService, GunShootingService, InteractableService);
        var settingsState = new SettingsState(this, PlayerService, GameTimeService, GunShootingService, InteractableService);

        // Define transitions
        At(gameplayState, uiWindowState, new FuncPredicate(() => isUIWindowOpen));
        //At(uiWindowState, gameplayState, new FuncPredicate(() => !isUIWindowOpen));

        Any(settingsState, new FuncPredicate(() => isSettingsOpen));
        Any(gameplayState, new FuncPredicate(() => !isSettingsOpen && !isUIWindowOpen));

        gameStateMachine.SetState(gameplayState);
    }

    private void Update()
    {
        gameStateMachine.Update();

        if (!isUIWindowOpen && !isSettingsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                playerInventoryWindowContainer.Interact();
            }
        }

        
    }

    public void Die()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    [Button]
    public void OpenUIWindow()
    {
        isUIWindowOpen = true;
    }

    [Button]
    public void CloseUIWindow()
    {
        isUIWindowOpen = false;
    }

    [Button]
    public void OpenSettingsWindow()
    {
        isSettingsOpen = true;
    }

    [Button]
    public void CloseSettingsWindow()
    {
        isSettingsOpen = false;
    }

    private void At(IState from, IState to, IPredicate condition) => gameStateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => gameStateMachine.AddAnyTransition(to, condition);
}
