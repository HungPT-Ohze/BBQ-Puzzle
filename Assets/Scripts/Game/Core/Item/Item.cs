using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private ItemType type;

    [Header("Component")]
    [SerializeField] private ObjectSettings objectSettings;

    public ItemType Type => type;
    public ObjectSettings ObjectSettings => objectSettings;

    public void Set(int id)
    {
        objectSettings.Id = $"{id}";
        this.gameObject.name = $"{type.ToString()}_{id}";
    }
}
