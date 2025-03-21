using Cysharp.Threading.Tasks;
using Lean.Pool;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private LevelSO config;

    [Header("Holder")]
    [SerializeField] private Transform containerHolder;
    [SerializeField] private Transform itemHolder;

    [Header("List container")]
    [SerializeField] private List<Container> containers = new List<Container>();

    [Header("List item")]
    [ShowInInspector] private List<Item> itemsInGame = new List<Item>();

    // Private variable
    private DragDropManager dragDropManager;

    private async void Start()
    {
        // Init
        Init();

        // Container
        await UniTask.DelayFrame(1);
        AddContainerToDragDropManager();
        await UniTask.DelayFrame(1);
        SetupContainers();

        // Item
        await UniTask.DelayFrame(1);
        CreateItem();
        await UniTask.DelayFrame(3);
        AddItemToDragDropManager();

        // Drag drop setup
        await UniTask.DelayFrame(1);
        dragDropManager.Setup();

        // Create level
        await UniTask.WaitForSeconds(0.25f);
        RandomGenerateLevel();
    }

    public void Init()
    {
        dragDropManager = GamePlay.DragDropManager;
    }

    #region Container
    private void SetupContainers()
    {
        foreach (var container in containers)
        {
            container.Set();
        }
    }

    private void AddContainerToDragDropManager()
    {
        dragDropManager.AllPanels.Clear();

        foreach (var container in containers)
        {
            // Main slot
            foreach (var slot in container.Slots)
            {
                PanelSettings panelSettings = slot.GetComponent<PanelSettings>();
                dragDropManager.AllPanels.Add(panelSettings);
            }

            // Add slot of tray
            foreach (var tray in container.Trays)
            {
                foreach(var slot in tray.Slots)
                {
                    PanelSettings panelSettings = slot.GetComponent<PanelSettings>();
                    dragDropManager.AllPanels.Add(panelSettings);
                }
            }
        }
    }

    #endregion

    #region Item
    private void CreateItem()
    {
        int id = 0;

        foreach(var itemData in config.itemInGame)
        {
            Item item = ResourceManager.Instance.LoadItem((int) itemData.Type);

            for(int i = 0; i < itemData.amount; i++)
            {
                var obj = LeanPool.Spawn(item, itemHolder);
                itemsInGame.Add(obj);

                obj.Set(id);
                id++;
            }
        }
    }

    private void AddItemToDragDropManager()
    {
        foreach(var item in itemsInGame)
        {
            ObjectSettings objectSettings = item.GetComponent<ObjectSettings>();
            dragDropManager.AllObjects.Add(objectSettings);
        }
    }

    #endregion

    private void RandomGenerateLevel()
    {
        // Create slot list
        List<Slot> slotsNotFill = new List<Slot>();
        foreach(var panel in dragDropManager.AllPanels)
        {
            slotsNotFill.Add(panel.GetComponent<Slot>());
        }

        // Shuffed list item
        List<Item> shuffledItems = Utils.ListShuffled(itemsInGame);

        // Create list skip slot based on percent
        var skipSlots = new List<Tuple<int, int>>();

        foreach(var weight in config.skipSlotWeight)
        {
            var choice = new Tuple<int, int>(weight.skip, weight.percent);
            skipSlots.Add(choice);
        }

        // Move item to slot
        int indexItem = 0;

        for (int indexSlot = 0; indexSlot < slotsNotFill.Count; indexSlot += Utils.WeightedRandom(skipSlots))
        {
            Slot currentSlot = slotsNotFill[indexSlot].GetComponent<Slot>();

            if (indexItem >= shuffledItems.Count) break;

            Item currentItem = shuffledItems[indexItem];

            // Check if container filled by the last slot
            if (currentSlot.IsLastSlot)
            {
                Slot slot_1 = slotsNotFill[indexSlot - 2];
                Slot slot_2 = slotsNotFill[indexSlot - 1];

                if(slot_1.IsHasItem() && slot_2.IsHasItem())
                {
                    Item itemSlot_1 = slot_1.GetItem();
                    Item itemSlot_2 = slot_2.GetItem();

                    if(currentItem.Type.Equals(itemSlot_1.Type) && currentItem.Type.Equals(itemSlot_2.Type))
                    {
                        indexSlot++;
                        continue;
                    }
                }
            }

            AIDragDrop.DragDrop(currentItem.ObjectSettings.Id, currentSlot.PanelSettings.Id, true);
            indexItem++;
        }
    }

}
