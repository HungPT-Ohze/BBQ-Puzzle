using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Container : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int id;

    [Header("Slot")]
    [SerializeField] private List<Slot> slots = new List<Slot>();

    public int Id => id;
    public List<Slot> Slots => slots;

    public void Set()
    {
        foreach (Slot slot in slots)
        {
            slot.Set(this);
        }

        this.gameObject.name = $"Container_{id}";
    }    

    public bool CheckAllSlot()
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

    public void OnItemDropped()
    {
        bool check = CheckAllSlot();

        if(check)
        {
            Debug.Log("OKe");
        }
    }
}
