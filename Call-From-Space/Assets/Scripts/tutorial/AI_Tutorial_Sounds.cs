using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Tutorial_Sounds : MonoBehaviour
{
    public AudioSource audiosource;
    [Header("AI Audio Clips")]
    public AudioClip ExplosionSpawn;
    public AudioClip InitiateLockDown;
    public AudioClip WarnLockDown;
    public AudioClip EndLockDown;
    public AudioClip oxygen_online;
    public AudioClip low_oxygen;
    public AudioClip critical_oxygen;
    public AudioClip AutopilotEnganged;
    public AudioClip StasisDestination;

    [Header("Player Audio Clips")]
    public AudioClip INeedMySuit;
    public AudioClip INeedPower;
    public AudioClip INeedRadio;
    public AudioClip WrongWay;
    public AudioClip RefillOxygen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Play()
    {
        audiosource.Pause();
        audiosource.clip = null;
    }

    public void PlayINeedMySuit()
    {
        Play();
        audiosource.clip = INeedMySuit;
        audiosource.Play();
    }

    public void PlayRefillOxygen()
    {
        Play();
        audiosource.clip = RefillOxygen;
        audiosource.Play();
    }

    public void PlayWrongWay()
    {
        Play();
        audiosource.clip = WrongWay;
        audiosource.Play();
    }
    public void PlayINeedRadio()
    {
        Play();
        audiosource.clip = INeedRadio;
        audiosource.Play();
    }
    public void PlayINeedPower()
    {
        Play();
        audiosource.clip = INeedPower;
        audiosource.Play();
    }

    public void PlayExplosionSpawn()
    {
        Play();
        audiosource.PlayOneShot(ExplosionSpawn);
    }

    public void PlayInitiateLockDown()
    {
        Play();
        audiosource.PlayOneShot(InitiateLockDown);
    }

    public void PlayWarnLockDown()
    {
        Play();
        audiosource.PlayOneShot(WarnLockDown);
    }

    public void PlayEndLockDown()
    {
        Play();
        audiosource.PlayOneShot(EndLockDown);
    }

    public void Playoxygen_online()
    {
        Play();
        audiosource.PlayOneShot(oxygen_online);
    }

    public void Playlow_oxygen()
    {
        Play();
        audiosource.PlayOneShot(low_oxygen);
    }

    public void Playcritical_oxygen()
    {
        Play();
        audiosource.PlayOneShot(critical_oxygen);
    }

    public void PlayAutopilotEnganged()
    {
        Play();
        audiosource.PlayOneShot(AutopilotEnganged);
        //audiosource.PlayOneShot(AutopilotEnganged);
    }

    public void PlayStasisDestination()
    {
        Play();
        audiosource.clip = StasisDestination;
        audiosource.Play();
    }



}