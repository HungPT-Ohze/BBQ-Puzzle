public interface IDataSave
{
    string Key { get; set; }

    void Init();

    void Fix();

    void Save();

    void Clear();
}
