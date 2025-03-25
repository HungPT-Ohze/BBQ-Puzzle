using com.homemade.pattern.singleton;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoSingleton<TickManager>
{
    private Dictionary<string, TickSystem> tickSystems = new Dictionary<string, TickSystem>();

    public TickSystem CreateTickSystem(string id, float tickRate, bool startImmediately = true)
    {
        if (tickSystems.ContainsKey(id))
        {
            Debug.LogWarning($"Tick System with ID {id} already exists.");
            return tickSystems[id];
        }

        GameObject tickObject = new GameObject($"TickSystem_{id}");
        tickObject.transform.SetParent(transform);
        TickSystem newTickSystem = tickObject.AddComponent<TickSystem>();
        tickSystems.Add(id, newTickSystem);

        if(startImmediately)
        {
            tickSystems[id].StartTicking(tickRate);
        }

        return tickSystems[id];
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
