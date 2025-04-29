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

    public bool acceptingInsanityIncrease;

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

        acceptingInsanityIncrease = true;
        maxCaught = 3;
        minimumTimesCaught = 0;
    }

    public void setMinimumTimesCaught(int minimumTimesCaught)
    {
        this.minimumTimesCaught = minimumTimesCaught;
    }

    public void Reset()
    {
        currentInsanity = 0f;
        timesCaught = minimumTimesCaught;
    }

    public void IncreaseInsanity(float insanityIncrease)
    {
        if (!acceptingInsanityIncrease) return;

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
        acceptingInsanityIncrease = false;
        timesCaught++;
        MaxInsanity?.Invoke();
    }
}
