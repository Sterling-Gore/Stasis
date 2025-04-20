using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Notify();

public class InsanityMeter : MonoBehaviour
{
    private static InsanityMeter _instance;
    public static InsanityMeter Instance { get { return _instance; } }

    public event Notify MaxInsanity;

    public float currentInsanity { get; private set; }
    public int timesCaught { get; private set; }
    public int maxCaught { get; private set; }

    int minimumTimesCaught;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        maxCaught = 3;
        minimumTimesCaught = 0;
    }

    public void setMinimumTimesCaught(int minimumTimesCaught)
    {
        minimumTimesCaught = this.minimumTimesCaught;
    }

    public void Reset()
    {
        currentInsanity = 0f;
        timesCaught = minimumTimesCaught;
    }

    public void IncreaseInsanity(float insanityIncrease)
    {
        currentInsanity = Mathf.Clamp(currentInsanity + insanityIncrease, 0, 100);
        Debug.Log("Current Insanity: " + currentInsanity);
        if (currentInsanity == 100)
        {
            currentInsanity = 0;
            OnMaxInsanity();
        }
    }

    void OnMaxInsanity()
    {
        timesCaught++;
        MaxInsanity?.Invoke();
    }
}
