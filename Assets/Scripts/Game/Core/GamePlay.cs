using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GamePlay : MonoSingleton<GamePlay>
{
    [Header("Component")]
    public DragDropManager dragDropManager;

    // Private variables
    private bool isDoneCreateGamePlay = false;
    public bool IsDoneCreateGamePlay => isDoneCreateGamePlay;

    private void Start()
    {
#if UNITY_EDITOR
        dragDropManager.TargetPlatform = DragDropManager.Platforms.PC;
#elif UNITY_ANDROID || UNITY_IOS
        dragDropManager.TargetPlatform = DragDropManager.Platforms.Mobile;
#endif

        // Start game
    }

}
