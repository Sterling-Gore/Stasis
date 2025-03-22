using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerDarkFigure : MonoBehaviour
{
    public GameObject darkFigure;
    public bool showingCreature = false; 
    public Flashlight leftLight;
    public Flashlight rightLight;
    public AudioSource audioSource;

    public GameObject EndingTrigger;
    public bool playAudioOnSpawn;
    public bool turnOffFlashlight;
    public bool flashLightScaresFigure;
    public bool breakLightAtExit;
    

    // Update is called once per frame
    void Update()
    {
       if(showingCreature && flashLightScaresFigure && Input.GetKeyUp(KeyCode.F))
       {
            despawnFigure();
       } 
    }

    public void spawnFigure()
    {
        darkFigure.SetActive(true);
        if(playAudioOnSpawn)
        {
            PlayScaryAudio();
        }
        EndingTrigger.SetActive(true);
        StartCoroutine(spawnDelay());
    }
    public void despawnFigure()
    {
        darkFigure.SetActive(false);
        showingCreature = false;
        if(breakLightAtExit)
        {
            leftLight.turnOffLight();
            rightLight.turnOffLight();
        }
    }

    IEnumerator spawnDelay()
    {
        yield return new WaitForSeconds(1.25f);
        if(turnOffFlashlight)
        {
            leftLight.turnOffLight();
            rightLight.turnOffLight();
        }
        showingCreature = true;

    }

    public void PlayScaryAudio()
    {
        audioSource.Play();
    }
}
