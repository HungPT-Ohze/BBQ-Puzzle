using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Config/Level", order = 2)]
public class LevelSO : ScriptableObject
{
    [Header("Info")]
    public int levelID;

    [Header("List data")]
    public List<LevelData> data;

    [Title("Tool")]
    [Button("Check is data correct")]
    private void CheckData()
    {
        isNotCorrect = false;

        // Check for duplicate
        HashSet<ItemType> set = new HashSet<ItemType>();

        foreach (var level in data)
        {
            if(!set.Add(level.itemType))
            {
                isNotCorrect = true;
                break;
            }
        }

        checkMessage = CheckMessageInfo();
    }

    [Title("Message")]
    public bool showMessage = true;
    private bool isNotCorrect;

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
        return isNotCorrect ? Color.red : Color.green;
    }

    private string CheckMessageInfo()
    {
        if(isNotCorrect)
        {
            return "You need to check again the data";
        }
        else
        {
            return "Okela!";
        }
    }
}

[System.Serializable]
public class LevelData
{
    public ItemType itemType;
    public int amount;
}