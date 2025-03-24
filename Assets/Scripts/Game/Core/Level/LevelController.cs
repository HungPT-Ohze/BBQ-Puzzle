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

    [Title("List of slot")]
    [Title("Main slot", horizontalLine: false)]
    [ShowInInspector] private List<Slot> mainSlots = new List<Slot>();
    [Title("Support slot")]
    [ShowInInspector] private List<Slot> supportSlots = new List<Slot>();

    // Private variable
    private DragDropManager dragDropManager;

    public LevelSO Config => config;

    private async void Start()
    {
        // Init
        Init();

        // Container
        await UniTask.DelayFrame(1);
        AddContainerToDragDropManager();
        await UniTask.DelayFrame(1);
        SetupContainers();
        await UniTask.DelayFrame(1);
        CreateListOfSlot();

        // Item
        await UniTask.DelayFrame(1);
        CreateItem();
        await UniTask.DelayFrame(3);
        AddItemToDragDropManager();

        // Drag drop setup
        await UniTask.DelayFrame(1);
        dragDropManager.Setup();

        // Create level
        await UniTask.DelayFrame(1);
        RandomGenerateLevel();
        await UniTask.WaitForSeconds(0.1f);
        SetTrayBehaviour();

        // Check level
        await UniTask.DelayFrame(2);
        while (!CheckLevel())
        {
            UniTask resetTask = ResetLevel();
            await UniTask.WhenAll(resetTask);
        }
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

    private void ReInitContainers()
    {
        foreach (var container in containers)
        {
            container.ReInit();
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

                if(!itemsInGame.Contains(obj))
                {
                    itemsInGame.Add(obj);
                }

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

    private void CreateListOfSlot()
    {
        // Main slot
        foreach(var container in containers)
        {
            mainSlots.AddRange(container.Slots);

            foreach(var tray in container.Trays)
            {
                supportSlots.AddRange(tray.Slots);
            }
        }
    }

    private void RandomGenerateLevel()
    {
        // Shuffed list item
        List<Item> shuffledItems = new List<Item>(); 
        shuffledItems.AddRange(itemsInGame);
        shuffledItems = Utils.ListShuffled(shuffledItems);

        // Move item to main slots
        FillItemToSlot(mainSlots, shuffledItems);

        // Remove the item in main slot
        List<Item> itemsNotInMainSlot = new List<Item>();

        foreach(var slot in mainSlots)
        {
            Item item = shuffledItems.Find(i => i.ObjectSettings.Id == slot.PanelSettings.ObjectId);
            shuffledItems.Remove(item);
        }

        // Move item to support slots
        FillItemToSlot(supportSlots, shuffledItems);
    }

    private void FillItemToSlot(List<Slot> slots, List<Item> items)
    {
        // Create list skip slot based on percent
        var skipSlots = new List<Tuple<int, int>>();

        foreach (var weight in config.skipSlotWeight)
        {
            var choice = new Tuple<int, int>(weight.skip, weight.percent);
            skipSlots.Add(choice);
        }

        // Move item to slot
        int indexItem = 0;

        for (int indexSlot = 0; indexSlot < slots.Count; indexSlot += Utils.WeightedRandom(skipSlots))
        {
            Slot currentSlot = slots[indexSlot].GetComponent<Slot>();

            if (indexItem >= items.Count) break;
            if (currentSlot.IsHasItem()) continue;

            Item currentItem = items[indexItem];

            // Check if container filled by the last slot
            if (currentSlot.IsLastSlot)
            {
                Slot slot_1 = slots[indexSlot - 2];
                Slot slot_2 = slots[indexSlot - 1];

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

    private void SetIgnoreAllTray(bool isIgnore)
    {
        foreach (var slot in supportSlots)
        {
            slot.PanelSettings.Ignore = isIgnore;
            slot.PanelSettings.LockObject = PanelSettings.ObjectLockStates.LockObject;
        }
    }

    private void SetTrayBehaviour()
    {
        // Tray can not drop item
        SetIgnoreAllTray(true);

        // Tray have item check
        foreach (var container in containers)
        {
            foreach (var tray in container.Trays)
            {
                bool isHasItem = tray.IsHasItem();
                tray.gameObject.SetActive(isHasItem);
            }
        }
    }

    private void OpenAllTray()
    {
        foreach (var container in containers)
        {
            foreach (var tray in container.Trays)
            {
                tray.gameObject.SetActive(true);
            }
        }
    }

    private void ActiveAllItem()
    {
        foreach(var item in itemsInGame)
        {
            item.gameObject.SetActive(true);
        }
    }

    #region Reset Level
    [Title("Tool")]
    [Button]
    public async void ResetGamePlay()
    {
        do
        {
            UniTask resetTask = ResetLevel();
            await UniTask.WhenAll(resetTask);
        }
        while (!CheckLevel());
    }

    private async UniTask ResetLevel()
    {
        // Reset
        ActiveAllItem();
        await UniTask.DelayFrame(1);

        // Manager
        dragDropManager.AllObjects.Clear();
        AddItemToDragDropManager();
        await UniTask.DelayFrame(1);
        DragDropManager.ResetScene();

        // Container
        await UniTask.DelayFrame(1);
        ReInitContainers();
        await UniTask.DelayFrame(1);
        OpenAllTray();
        SetIgnoreAllTray(false);

        // Drag drop setup
        await UniTask.DelayFrame(1);

        // Create level
        await UniTask.DelayFrame(1);
        RandomGenerateLevel();
        await UniTask.WaitForSeconds(0.1f);
        SetTrayBehaviour();

        await UniTask.CompletedTask;
    }

    private bool CheckLevel()
    {
        int numberItemInSlot = 0;

        foreach(var slot in mainSlots)
        {
            if(slot.IsHasItem())
            {
                numberItemInSlot += 1;
            }
        }

        if(numberItemInSlot == mainSlots.Count)
        {
            return false;
        }

        foreach (var slot in supportSlots)
        {
            if (slot.IsHasItem())
            {
                numberItemInSlot += 1;
            }
        }

        if(numberItemInSlot < itemsInGame.Count)
        {
            return false;
        }

        return true;
    }

    #endregion
}
