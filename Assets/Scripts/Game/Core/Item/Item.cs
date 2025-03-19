using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private ItemType type;

    [Header("Component")]
    [SerializeField] private ObjectSettings dragSetting;

    public ItemType Type => type;

    public void Set()
    {

    }
}
