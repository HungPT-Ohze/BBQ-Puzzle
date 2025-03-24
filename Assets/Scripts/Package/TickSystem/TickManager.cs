using com.homemade.pattern.singleton;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoSingleton<TickManager>
{
    private Dictionary<string, TickSystem> tickSystems = new Dictionary<string, TickSystem>();

    public void CreateTickSystem(string id, float tickRate)
    {
        if (tickSystems.ContainsKey(id))
        {
            Debug.LogWarning($"Tick System with ID {id} already exists.");
            return;
        }

        GameObject tickObject = new GameObject($"TickSystem_{id}");
        tickObject.transform.SetParent(transform);
        TickSystem newTickSystem = tickObject.AddComponent<TickSystem>();
        newTickSystem.StartTicking(tickRate);
        tickSystems[id] = newTickSystem;
    }

    public void RemoveTickSystem(string id)
    {
        if (tickSystems.ContainsKey(id))
        {
            Destroy(tickSystems[id].gameObject);
            tickSystems.Remove(id);
        }
    }

    public TickSystem GetTickSystem(string id)
    {
        return tickSystems.ContainsKey(id) ? tickSystems[id] : null;
    }
}
