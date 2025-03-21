using UnityEngine;

public class Slot : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int slotIndex;
    [SerializeField] private bool isLastSlot;

    [Header("Component")]
    [SerializeField] private PanelSettings panelSettings;

    private Container container;
    private Tray tray;

    public PanelSettings PanelSettings => panelSettings;
    public bool IsLastSlot => isLastSlot;

    // Setup in container
    public void Set(Container container)
    {
        this.container = container;
        string id = $"Slot_Con_{container.Id}_{slotIndex}";
        panelSettings.Id = id;
        this.gameObject.name = id;

        panelSettings.OnObjectDropped.AddListener(OnObjectDropped);
        panelSettings.OnObjectExit.AddListener(OnObjectExit);
    }

    // Setup in tray
    public void Set(Tray tray)
    {
        this.tray = tray;
        string id = $"Slot_Con_{tray.ContainerId}_Tray_{tray.Id}_{slotIndex}";
        panelSettings.Id = id;
        this.gameObject.name = id;
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

        ObjectSettings obj = GamePlay.DragDropManager.AllObjects.Find(i => i.Id == panelSettings.ObjectId);
        return obj.GetComponent<Item>();
    }

    public void OnObjectDropped()
    {
        
    }

    public void OnObjectExit()
    {
        
    }
}
