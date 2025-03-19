using com.homemade.pattern.singleton;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GamePlay : MonoSingleton<GamePlay>
{
    [Header("Component")]
    public ContainerManager containerManager;
    public DragDropManager dragDropManager;

    private void Start()
    {
#if UNITY_EDITOR
        dragDropManager.TargetPlatform = DragDropManager.Platforms.PC;
#elif UNITY_ANDROID || UNITY_IOS
        dragDropManager.TargetPlatform = DragDropManager.Platforms.Mobile;
#endif

        // Start game
        CreateGamePlay();
    }

    public async void CreateGamePlay()
    {
        containerManager.Setup();
        await UniTask.DelayFrame(1);

        containerManager.AddToDragDropManager();
        await UniTask.DelayFrame(1);

        dragDropManager.Setup();
    }
}
