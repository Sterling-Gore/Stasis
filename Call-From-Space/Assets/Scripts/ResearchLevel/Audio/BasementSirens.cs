using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementSirens : MonoBehaviour
{
    public AudioSource audioSource;
    public bool isComplete = false;
    void Start()
    {
        audioSource.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isComplete)
            audioSource.Play();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            audioSource.Stop();
    }

    public void complete()
    {
        isComplete = true;
        audioSource.Stop();
    }
}
