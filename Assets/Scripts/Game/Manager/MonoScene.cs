using com.homemade.pattern.singleton;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class MonoScene : MonoSingleton<MonoScene>
{
    private AsyncOperationHandle<SceneInstance> mapHandle;

    public void LoadMainScene(Action onDone = null)
    {
        string name = NameSceneEnum.Main.ToString();
        var operation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
        operation.completed += (op) =>
        {
            onDone?.Invoke();
        };
    }

    public void LoadGameScene(Action onDone = null)
    {
        // Start a coroutine to handle the loading process and setting active scene
        StartCoroutine(LoadGameSceneIEnum(onDone));
    }

    private IEnumerator LoadGameSceneIEnum(Action onDone)
    {
        string name = NameSceneEnum.Gameplay.ToString();

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        // Wait until the asynchronous scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SetActiveScene(NameSceneEnum.Gameplay);

        onDone?.Invoke();
    }

    public void RemoveGameScene()
    {
        SetActiveScene(NameSceneEnum.Main);

        string gameSceneName = NameSceneEnum.Gameplay.ToString();
        Scene gameScene = SceneManager.GetSceneByName(gameSceneName);

        if (gameScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(gameScene);
        }
    }

    public void SetActiveScene(NameSceneEnum nameScene)
    {
        Scene scene = SceneManager.GetSceneByName(nameScene.ToString()); ;

        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene);
            Debug.Log("Scene set to active: " + scene.name);
        }
        else
        {
            Debug.LogError("Scene not valid or not loaded: " + scene);
        }
    }

    public void LoadHomeScene(Action onDone = null)
    {
        string key = $"Assets/Scenes/Map/Home/Home.unity";
        mapHandle = Addressables.LoadSceneAsync(key, LoadSceneMode.Additive, activateOnLoad: true);
        mapHandle.Completed += (op) =>
        {
            // Get the loaded scene instance
            SceneInstance loadedScene = op.Result;

            // Callback
            onDone?.Invoke();
        };
    }

}

public enum NameSceneEnum
{
    Splash,
    Main,
    Home,
    Gameplay,
}
