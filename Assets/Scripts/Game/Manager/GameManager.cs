using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

#if PRODUCTION
        Debug.unityLogger.logEnabled = false;
#endif

#if UNITY_EDITOR || DEVELOPMENT || STAGING
        if (Application.isEditor)
            Application.runInBackground = true;
#endif
    }


    private async void Start()
    {
        // Show Tittle screen

        // Inject service
        await UniTask.WaitUntil(() => GameService.Instance.IsInitialized);

        // Init data
        DataManager.Instance.Init();
        AppManager.Instance.Init();


        // Hide Tittle screen
    }
}
