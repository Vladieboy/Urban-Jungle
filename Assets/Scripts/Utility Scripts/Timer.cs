using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Range(0f, 60f), Min(0f)] public float  timeRemaining = 10;
    [Range(0f, 60f), Min(0f)] float  _defaultTimeRemaing = 10;
    public bool timerIsRunning = false;

    private void Start()
    {
        _defaultTimeRemaing = timeRemaining;
    }
    public void StartTimer()
    {
        timerIsRunning = true;
    }    
    public void StopTimer()
    {
        
        timerIsRunning = false;
    }

    public void ResetTimer()
    {
        timeRemaining = _defaultTimeRemaing;
    }

    void Update()
    {
        if (timerIsRunning)
        {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0) timeRemaining = 0;
        }
            else
            {
                Debug.Log("timer run out");

                //GetComponent<EnemyAI>().EnemyAlertPhase = EnemyAI.EnemyStatus.Alert;
                //timeRemaining = 10;
                timerIsRunning = false;
            }
        }
    }
}
