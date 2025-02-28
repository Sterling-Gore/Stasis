using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public OxygenSystem O2System;
    public AudioClip[] FootSteps;
    // Start is called before the first frame update
    public AudioSource[] playerTakeDamageSounds;
    
    //AudioSource ambiance;
    public AudioSource breathing;
    public AudioSource choking;
    public AudioSource walking;

    public speedometer speedo;

    bool startChoke = true; 
    void Start()
    {


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
            //Debug.Log("PLAY");
        }
        else{
            //Debug.Log("STOP");
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


    }



    public void AudibleDamage(float damageAmount)
    {
        int randomIndex = Random.Range(0, playerTakeDamageSounds.Length);
        AudioSource playerHurtSound = playerTakeDamageSounds[randomIndex];
        if( damageAmount != 10f) //this is when oxygen gone
            playerHurtSound.Play();
    }
}
