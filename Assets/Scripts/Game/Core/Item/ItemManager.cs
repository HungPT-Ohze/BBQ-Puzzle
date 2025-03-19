using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [ShowInInspector]
    private Dictionary<int, Item> itemInGame = new Dictionary<int, Item>();

    public void Init()
    {

    }   
    
    
}
