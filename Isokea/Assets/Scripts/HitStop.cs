using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField]
    float minTimeBetweenWaits;
    bool waiting;
    float timer;
    private void Update()
    {
        if (timer >= 0)
            timer -= Time.deltaTime;
    }
    public void Stop(float duration)
    {
        if (waiting || timer > 0)
            return;
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        timer = minTimeBetweenWaits;
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
