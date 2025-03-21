using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "Config/Level", order = 2)]
public class LevelSO : ScriptableObject
{
    [Header("Info")]
    public int levelID;
    public int amountSlot;

    [Header("Weight skip slot")]
    public List<SkipSlotWeight> skipSlotWeight;

    [Header("List item")]
    public List<LevelItem> itemInGame;

    [Title("Tool")]
    [Button("Check is data correct")]
    private void OnClickCheck()
    {
        bool checkItem = CheckForItem();
        bool checkItemNotInSlot = CheckForItemNotInSlot();

        isDataCorrect = checkItem && checkItemNotInSlot;

        checkMessage = CheckMessageInfo();
    }

    private bool CheckForItem()
    {
        HashSet<ItemType> set = new HashSet<ItemType>();

        foreach (var item in itemInGame)
        {
            // Check for duplicate
            if (!set.Add(item.Type))
            {
                return false;
            }

            // Check for amount
            if (!(item.amount % 3 == 0))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckForItemNotInSlot()
    {
        skipSlotWeight.Sort();

        int largestSkip = skipSlotWeight[skipSlotWeight.Count - 1].skip;
        int amountItem = 0;

        foreach (var item in itemInGame)
        {
            amountItem += item.amount;
        }

        bool check = (amountSlot - amountItem) / largestSkip >= 2;

        return check;
    }

    [Title("Message")]
    public bool showMessage = true;
    private bool isDataCorrect;

    [ShowIf("showMessage")]
    [GUIColor("GetMessageColor")]
    [InfoBox("Info check data", InfoMessageType.Info)]
    public string checkMessage = "This is where message show!";

    public GUIContent GetMessageContent()
    {
        return new GUIContent(checkMessage);
    }

    private Color GetMessageColor()
    {
        return isDataCorrect ? Color.green : Color.red;
    }

    private string CheckMessageInfo()
    {
        if(isDataCorrect)
        {
            return "Okela!";
        }
        else
        {
            return "You need to check again the data";
        }
    }

}

[Serializable]
public class LevelItem
{
    public ItemType Type;
    public int amount;
}

[Serializable]
public class SkipSlotWeight : IComparable<SkipSlotWeight>
{
    [MinValue(1)] public int skip;
    [MinValue(0)][MaxValue(100)] public int percent;

    public int CompareTo(SkipSlotWeight other)
    {
        return skip.CompareTo(other.skip);
    }
}
