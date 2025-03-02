using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Tutorial_Sounds : MonoBehaviour
{
    public AudioSource audiosource;
    [Header("Audio Clips")]
    public AudioClip ExplosionSpawn;
    public AudioClip InitiateLockDown;
    public AudioClip WarnLockDown;
    public AudioClip EndLockDown;
    public AudioClip oxygen_online;
    public AudioClip low_oxygen;
    public AudioClip critical_oxygen;
    public AudioClip AutopilotEnganged;
    public AudioClip StasisDestination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayExplosionSpawn()
    {
        audiosource.PlayOneShot(ExplosionSpawn);
    }

    public void PlayInitiateLockDown()
    {
        audiosource.PlayOneShot(InitiateLockDown);
    }

    public void PlayWarnLockDown()
    {
        audiosource.PlayOneShot(WarnLockDown);
    }

    public void PlayEndLockDown()
    {
        audiosource.PlayOneShot(EndLockDown);
    }

    public void Playoxygen_online()
    {
        audiosource.PlayOneShot(oxygen_online);
    }

    public void Playlow_oxygen()
    {
        audiosource.PlayOneShot(low_oxygen);
    }

    public void Playcritical_oxygen()
    {
        audiosource.PlayOneShot(critical_oxygen);
    }

    public void PlayAutopilotEnganged()
    {
        audiosource.PlayOneShot(AutopilotEnganged);
    }

    public void PlayStasisDestination()
    {
        audiosource.PlayOneShot(StasisDestination);
    }



}