using Sirenix.OdinInspector;

[System.Serializable]
public abstract class BaseDataSave : IDataSave
{
    public string Key { get => key; set => key = value; }
    private string key;

    public virtual void Init()
    {

    }

    public virtual void Fix()
    {

    }

    [Button]
    public virtual void Save()
    {
        DataManager.Save.Save(key, this);
    }

    public virtual void Clear()
    {

    }

}

