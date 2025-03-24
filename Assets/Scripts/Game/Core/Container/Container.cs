using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Container : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int id;

    [Header("Slot")]
    [SerializeField] private List<Slot> slots = new List<Slot>();

    [Header("Tray")]
    [SerializeField] private List<Tray> trays = new List<Tray>();

    private int idTrayOnTop = 0;

    public int Id => id;
    public List<Slot> Slots => slots;
    public List<Tray> Trays => trays;

    public void Set()
    {
        idTrayOnTop = trays[trays.Count - 1].Id;

        // Main slots
        foreach (Slot slot in slots)
        {
            slot.Set(this);
        }

        // Tray
        foreach (Tray tray in trays)
        {
            tray.Set(this);
        }

        this.gameObject.name = $"Container_{id}";
    }

    public void ReInit()
    {
        idTrayOnTop = trays[trays.Count - 1].Id;
    }

    private bool CheckAllSlot()
    {
        // Not fill all slot
        bool checkNotFill = slots.Any(s => string.IsNullOrEmpty(s.PanelSettings.ObjectId));
        if(checkNotFill) return false;

        // Not correct type
        Item firstSlotItem = slots[0].GetItem();

        for(int i = 1; i < slots.Count; i++)
        {
            if(slots[i].GetItem().Type != firstSlotItem.Type)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckIfContainHasItem()
    {
        foreach(var slot in slots)
        {
            if (slot.IsHasItem()) 
                return true;
        }

        return false;
    }

    private void CollectItem()
    {
        foreach(var slot in slots)
        {
            var item = slot.GetItem();
            DragDropManager.RemoveObject(item.ObjectSettings);

            item.gameObject.SetActive(false);
        }

        Debug.Log("Oke");
    }

    public void OnItemDropped()
    {
        bool isFull = CheckAllSlot();

        if (!isFull) return;

        CollectItem();

        MoveItemFromTrayToContainer();
    }

    public void OnItemExit()
    {
        bool isHasItem = CheckIfContainHasItem();

        if (isHasItem) return;

        MoveItemFromTrayToContainer();
    }

    private void MoveItemFromTrayToContainer()
    {
        if (idTrayOnTop < 0) return;

        Tray tray = trays[idTrayOnTop];

        for (int i = 0; i < slots.Count; i++)
        {
            Slot slotInTray = tray.Slots[i];

            if (slotInTray.IsHasItem())
            {
                slotInTray.PanelSettings.LockObject = PanelSettings.ObjectLockStates.UseObjectSettings;

                string panelId = slots[i].PanelSettings.Id;
                string objectId = slotInTray.PanelSettings.ObjectId;

                AIDragDrop.DragDrop(objectId, panelId);
            }
        }

        tray.gameObject.SetActive(false);

        idTrayOnTop--;
    }

}
