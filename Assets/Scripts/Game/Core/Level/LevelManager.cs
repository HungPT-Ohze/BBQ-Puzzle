using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Transform levelHolder;

    private LevelController currentLevel;

    public void CreateLevel()
    {
        LoadLevelAsset(0, result =>
        {
            currentLevel = result.GetComponent<LevelController>();
        });
    }

    public void LoadLevelAsset(int levelID, Action<GameObject> onDone = null)
    {
        GameObject levelObj = null;
        string path = $"Assets/Prefabs/Level/Level_{levelID}.prefab";
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(path);

        handle.Completed += (AsyncOperationHandle<GameObject> op) =>
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                levelObj = Instantiate(handle.Result, levelHolder, false);
                onDone?.Invoke(levelObj);
            }
            else
            {
                Debug.Log("Load fail level " +  levelID);
            }
        };
    }

    public void ResetLevel()
    {
        currentLevel.ResetGamePlay();
    }
}
