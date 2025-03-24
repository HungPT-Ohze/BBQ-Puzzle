using com.homemade.pattern.observer;
using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GamePlay : MonoSingleton<GamePlay>
{
    [Header("Manager")]
    [SerializeField] private DragDropManager dragDropManager;
    [SerializeField] private LevelManager levelManager;

    [Header("Component")]
    [SerializeField] private GameObject eventSysObj;

    // Public static
    public static DragDropManager DragDropManager => Instance.dragDropManager;
    public static LevelManager LevelManager => Instance.levelManager;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        // To detect platforms
#if UNITY_EDITOR
        dragDropManager.TargetPlatform = DragDropManager.Platforms.PC;
#elif UNITY_ANDROID || UNITY_IOS
        dragDropManager.TargetPlatform = DragDropManager.Platforms.Mobile;
#endif

        // Start game
        StartGame();
    }

    public void Setup()
    {
        eventSysObj.SetActive(false);
    }

    public void StartGame()
    {
        levelManager.CreateLevel();

        this.PostEvent(EventID.StartGame);
    }

    public void PauseGame()
    {

    }

    public void EndGame()
    {

    }

    public void ResetGamePlay()
    {
        levelManager.ResetLevel();
    }
}
