using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEnterTrigger : MonoBehaviour
{
    public GameObject doorBlocker;
    public Animator elevatorAnimation;
    public leavingWorkshop exitScript;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip ElevatorDepatureClip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorBlocker.SetActive(true);
            elevatorAnimation.SetTrigger("shut");
            audioSource.Pause();
            audioSource.clip = ElevatorDepatureClip;
            audioSource.Play();
            exitScript.leaveScene();
            gameObject.SetActive(false);


        }
    }
}
