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
    
    [Header("Screen Flash")]
    public GameObject scareImage;
    public AudioSource  scareAudio;

    [Header("Piano Ambiance")]
    public AudioSource invertedPianoAmbiance;
    public bool PlayPiano;
    

    // Update is called once per frame
    void Update()
    {
       if(showingCreature && flashLightScaresFigure && Input.GetKeyUp(KeyCode.F))
       {
            despawnFigure();
       } 
    }

    public void spawnFigure(bool UseImage = false, bool UseAudio = false)
    {
        darkFigure.SetActive(true);
        if(playAudioOnSpawn)
        {
            PlayScaryAudio();
        }
        EndingTrigger.SetActive(true);
        StartCoroutine(spawnDelay());

        StartCoroutine(ScareImage(UseImage, UseAudio));
    }
    public void despawnFigure(bool UseImage = false, bool UseAudio = false)
    {
        darkFigure.SetActive(false);
        showingCreature = false;
        if(breakLightAtExit)
        {
            leftLight.turnOffLight();
            rightLight.turnOffLight();
        }

        StartCoroutine(ScareImage(UseImage, UseAudio));
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
        if(PlayPiano)
            invertedPianoAmbiance.Play();
    }

    IEnumerator ScareImage(bool UseImage, bool UseAudio)
    {
        if(UseAudio)
        {
            scareAudio.Play();
        }
        if(UseImage)
        {
            scareImage.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            scareImage.SetActive(false);
        }
    }
}
