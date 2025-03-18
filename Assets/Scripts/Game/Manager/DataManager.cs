using com.homemade.pattern.singleton;
using com.homemade.save.core;
using UnityEngine;

public class DataManager : MonoSingleton<DataManager>
{
    public static GameConfig Config;
    public static DatasaveManager Save;

    public void Init()
    {
        Config = Resources.Load<GameConfig>("Config/GameConfig");
        DatasaveManager.Instance.Init(this.transform);
    }

    public void ClearData()
    {
        IOSave.DeleteAll(Application.persistentDataPath);
        PlayerPrefs.DeleteAll();
        Debug.Log("Clear Save Data");
    }
}
