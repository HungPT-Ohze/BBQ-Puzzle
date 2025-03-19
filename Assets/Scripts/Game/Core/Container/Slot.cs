using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int slotIndex;
    [SerializeField] private SlotStatus status;

    [Header("Component")]
    [SerializeField] private PanelSettings panelSettings;

    private Container container;

    public PanelSettings PanelSettings => panelSettings;

    public void Set(Container container)
    {
        this.container = container;
        string id = $"Slot_{container.Id}_{slotIndex}";
        panelSettings.Id = id;
        this.gameObject.name = id;
    }

    public Item GetItem()
    {
        if(string.IsNullOrEmpty(panelSettings.ObjectId))
        {
            return null;
        }

        ObjectSettings obj = GamePlay.Instance.dragDropManager.AllObjects.Find(i => i.Id == panelSettings.ObjectId);
        return obj.GetComponent<Item>();
    }

}
