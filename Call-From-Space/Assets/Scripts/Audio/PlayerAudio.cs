using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public OxygenSystem O2System;
    public AudioClip[] FootSteps;
    // Start is called before the first frame update
    AudioSource[]  Audios;
    AudioSource walking;
    AudioSource sprinting;
    AudioSource crouching;
    //AudioSource ambiance;
    AudioSource breathing;
    AudioSource choking;

    public speedometer speedo;

    bool startChoke = true; 
    void Start()
    {
      Audios = gameObject.GetComponents<AudioSource>();
      walking = Audios[0];
      sprinting = Audios[1];
      crouching = Audios[2];
      //ambiance = Audios[3];
      breathing = Audios[3];
      choking = Audios[4];
      //ambiance.enabled = true;  

      speedo = gameObject.GetComponent<speedometer>();
    }

    

    // Update is called once per frame
    void Update()
    {
        
       // if(GetComponent<PlayerController>().UI_Value < 0)
       //     breathing.enabled = false;
       // else
       //     breathing.enabled = true;
        walking.pitch = speedo.speed / 3;
        if(!walking.isPlaying)
        {
            walking.PlayOneShot(FootSteps[Random.Range(0, FootSteps.Length)]);
            Debug.Log("PLAY");
        }
        else{
            Debug.Log("STOP");
        }
/*
       if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
       {
        if (Input.GetKey("left shift"))
            {
                walking.enabled = false;
                sprinting.enabled = true;
                crouching.enabled = false;
            }
            else if (Input.GetKey("left ctrl"))
            {
                walking.enabled = false;
                sprinting.enabled = false;
                crouching.enabled = true;
            }
            else{
                walking.enabled = true;
                sprinting.enabled = false;
                crouching.enabled = false;
            }
       } 
       else
        {
            walking.enabled = false;
            sprinting.enabled = false;
            crouching.enabled = false;
        }

        //breathing.volume = Mathf.Clamp(((1 - O2System.oxygenLevel / 100) - .50f) / 1.5f, 0, 1);
        if(O2System.LosingOxygen)
        {
            if(O2System.oxygenLevel == 0)
            {
                breathing.volume = 0;

                if(startChoke)
                    choking.Play();
                    startChoke = false;
            }
            else
            {
                startChoke = true;
                choking.Stop();
                breathing.volume = Mathf.Clamp(((1 - O2System.oxygenLevel / 100) - .50f) / 1.5f, 0, 1);
            }
        }
        else
            breathing.volume = 0;

        //starts at 50% oxygen, and the volume is divided by 1.5

*/
    }
}
