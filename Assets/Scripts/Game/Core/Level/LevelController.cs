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
        await UniTask.DelayFrame(1);

        // Container
        AddContainerToDragDropManager();
        await UniTask.DelayFrame(1);

        SetupContainers();
        await UniTask.DelayFrame(1);

        // Item
        CreateItem();
        await UniTask.DelayFrame(3);
        AddItemToDragDropManager();
        await UniTask.DelayFrame(1);

        // Drag drop setup
        dragDropManager.Setup();
        await UniTask.DelayFrame(5);

        RandomGenerateLevel();
    }

    public void Init()
    {
        dragDropManager = GamePlay.Instance.dragDropManager;
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
            foreach (var slot in container.Slots)
            {
                PanelSettings panelSettings = slot.GetComponent<PanelSettings>();
                dragDropManager.AllPanels.Add(panelSettings);
            }
        }
    }

    #endregion

    #region Item
    private void CreateItem()
    {
        int id = 0;

        foreach(var data in config.data)
        {
            Item item = ResourceManager.Instance.LoadItem((int) data.itemType);

            for(int i = 0; i < data.amount; i++)
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
        var skipSlots = new List<Tuple<int, int>>
        {
            Tuple.Create(1, 70), // 70% chance
            Tuple.Create(2, 20), // 20% chance
            Tuple.Create(3, 10)  // 10% chance
        };

        // Move item to slot
        int indexItem = 0;

        for (int indexSlot = 0; indexSlot < slotsNotFill.Count; indexSlot += Utils.WeightedRandom(skipSlots))
        {
            Slot currentSlot = slotsNotFill[indexSlot].GetComponent<Slot>();

            if (indexSlot >= shuffledItems.Count) break;

            Item currentItem = shuffledItems[indexItem];

            // Check if container filled by the last slot
            if (currentSlot.IsLastSlot)
            {
                Slot slot_1 = slotsNotFill[indexSlot - 2];
                Slot slot_2 = slotsNotFill[indexSlot - 1];

                if(slot_1.IsHasItem() && slot_2.IsHasItem())
                {
                    int tmpIndexItem = indexItem;
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
