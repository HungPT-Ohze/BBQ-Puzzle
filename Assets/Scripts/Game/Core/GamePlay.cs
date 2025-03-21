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
    

    private void Start()
    {
#if UNITY_EDITOR
        dragDropManager.TargetPlatform = DragDropManager.Platforms.PC;
#elif UNITY_ANDROID || UNITY_IOS
        dragDropManager.TargetPlatform = DragDropManager.Platforms.Mobile;
#endif

        // Start game
    }

    public void Setup()
    {
        eventSysObj.SetActive(false);
    }
}
