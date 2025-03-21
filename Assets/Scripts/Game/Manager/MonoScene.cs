using com.homemade.pattern.singleton;
using System;
using UnityEngine;
using System.Collections;
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

    public void LoadHomeScene(Action onDone = null)
    {
        // Start a coroutine to handle the loading process and setting active scene
        StartCoroutine(LoadHomeSceneIEnum(onDone));
    }

    private IEnumerator LoadHomeSceneIEnum(Action onDone)
    {
        string name = NameSceneEnum.Home.ToString();

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);

        // Wait until the asynchronous scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SetActiveScene(NameSceneEnum.Home);

        onDone?.Invoke();
    }

    public void RemoveScene(NameSceneEnum sceneEnum)
    {
        SetActiveScene(NameSceneEnum.Main);

        string sceneName = sceneEnum.ToString();
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.IsValid())
        {
            SceneManager.UnloadSceneAsync(scene);
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
}

public enum NameSceneEnum
{
    Splash,
    Main,
    Home,
    Gameplay,
}
