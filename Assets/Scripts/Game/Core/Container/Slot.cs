using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int slotIndex;
    [SerializeField] private bool isLastSlot;

    [Header("Component")]
    [SerializeField] private PanelSettings panelSettings;

    private Container container;

    public PanelSettings PanelSettings => panelSettings;
    public bool IsLastSlot => isLastSlot;

    public void Set(Container container)
    {
        this.container = container;
        string id = $"Slot_{container.Id}_{slotIndex}";
        panelSettings.Id = id;
        this.gameObject.name = id;

        panelSettings.OnObjectDropped.AddListener(OnObjectDropped);
        panelSettings.OnObjectExit.AddListener(OnObjectExit);
    }

    public bool IsHasItem()
    {
        return !string.IsNullOrEmpty(panelSettings.ObjectId);
    }

    public Item GetItem()
    {
        if(!IsHasItem())
        {
            return null;
        }

        ObjectSettings obj = GamePlay.Instance.dragDropManager.AllObjects.Find(i => i.Id == panelSettings.ObjectId);
        return obj.GetComponent<Item>();
    }

    public void OnObjectDropped()
    {
        
    }

    public void OnObjectExit()
    {
        
    }
}
