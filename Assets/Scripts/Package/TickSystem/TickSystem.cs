using System;
using UnityEngine;
using System.Collections;

public class TickSystem : MonoBehaviour
{
    public event Action<int> OnTick; // Event for subscribers

    private float tickRate;
    private int tickCount = 0;
    private bool isRunning = false;
    private bool isPaused = false;

    public void PauseTick() => isPaused = true;
    public void ResumeTick() => isPaused = false;

    public void StartTicking(float rate)
    {
        tickRate = rate;
        isRunning = true;
        StartCoroutine(TickCoroutine());
    }

    public void StopTicking()
    {
        isRunning = false;
        StopAllCoroutines();
    }

    private IEnumerator TickCoroutine()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(tickRate);
            if (!isPaused)
            {
                tickCount++;
                OnTick?.Invoke(tickCount);
            }
        }
    }
}
