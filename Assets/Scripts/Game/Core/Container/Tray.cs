using System.Collections.Generic;
using UnityEngine;

public class Tray : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private int id;

    [Header("Slot")]
    [SerializeField] private List<Slot> slots = new List<Slot>();

    private Container container;

    public int Id => id;
    public int ContainerId => container.Id;
    public List<Slot> Slots => slots;   

    public void Set(Container container)
    {
        this.container = container;

        foreach (Slot slot in slots)
        {
            slot.Set(this);
        }
    }

}
