using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private DragDropManager dragDropManager;

    [Header("List container")]
    [SerializeField] private List<Container> containers = new List<Container>();

    public void Setup()
    {
        foreach (var container in containers)
        {
            container.Set();
        }
    }

    public void AddToDragDropManager()
    {
        dragDropManager.AllPanels.Clear();

        foreach (var container in containers)
        {
            foreach(var slot in container.Slots)
            {
                PanelSettings panelSettings = slot.GetComponent<PanelSettings>();
                dragDropManager.AllPanels.Add(panelSettings);
            }
        }
    }

}
