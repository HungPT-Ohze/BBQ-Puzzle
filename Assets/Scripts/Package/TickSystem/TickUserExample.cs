using UnityEngine;

public class TickUserExample : MonoBehaviour
{
    private void Start()
    {
        // Create different tick systems
        TickManager.Instance.CreateTickSystem("AI", 0.5f);
        TickManager.Instance.CreateTickSystem("Network", 1.0f);

        // Subscribe to AI Tick System
        TickSystem aiTickSystem = TickManager.Instance.GetTickSystem("AI");
        if (aiTickSystem != null)
        {
            aiTickSystem.OnTick += AIUpdate;
        }
    }

    private void AIUpdate(int tickNumber)
    {
        Debug.Log($"AI Tick {tickNumber}: Running AI update...");
    }
}
