using com.homemade.pattern.singleton;
using com.homemade.save.core;
using System.Collections.Generic;
using UnityEngine;

public class DatasaveManager : MonoSingleton<DatasaveManager>
{
    // Service
    private ISaveService saveService;

    // Dictionary for store data
    private Dictionary<string, IDataSave> data = new Dictionary<string, IDataSave>();

    // Data save
    [SerializeField] private UserSave user;
    public UserSave User => user;

    public void Init(Transform parent = null)
    {
        DataManager.Save = this;

        if (parent)
            transform.SetParent(parent);

        saveService = GameService.Instance.GetService<ISaveService>();
        saveService.Saver = new PlayerPrefsSave();

        LoadAllData();
        FixAllData();
    }

    public void FixAllData()
    {
        foreach (var d in data.Values)
        {
            d.Fix();
        }
    }

    public void LoadAllData()
    {
        Load(ref user, typeof(UserSave).Name, new UserSave());
    }

    private void Load<T>(ref T referenceValue, string key, T defaultValue) where T : IDataSave
    {
        var tmp = defaultValue;
        if (saveService.Exists(key))
        {
            tmp = saveService.Load<T>(key);
        }
        else
        {
            tmp.Init();
        }

        tmp.Key = key;

        if (data.ContainsKey(key))
            data[key] = tmp;
        else
            data.Add(key, tmp);
        referenceValue = (T)data[key];
    }

    public void Save<T>(string key, T data)
    {
        saveService.Save(key, data);
    }

    public void SaveAll()
    {
        foreach (var d in data.Values)
        {
            d.Save();
        }
    }
}
