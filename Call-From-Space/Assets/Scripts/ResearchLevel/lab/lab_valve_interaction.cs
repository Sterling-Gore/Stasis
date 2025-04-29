using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lab_valve_interaction : Interactable
{
    public Animator animation;
    public AudioSource audioSource;
    public Collider valveCollider;
    public AudioClip[] valveSounds;
    public bool isComplete = false;
    public bool puzzleReady = false;
    public bool holdableReady = false;
    public int tertiaryBoolForValve = 0;
    



    public GameObject sparkle;
    public PrepareVial prepareVial;

    
    // Start is called before the first frame update
    void Start()
    {
        if( isComplete )
        {
            sparkle.SetActive(false);
        }
    }

    void Update()
    {
        if(tertiaryBoolForValve == 0)
        {
            if(puzzleReady && holdableReady)
            {
                tertiaryBoolForValve = 1;
                StartCoroutine(closeVat());
            }
        }
    }


    public override string GetDescription()
    {
        if (isComplete)
        {
            return "";
        }
        if(tertiaryBoolForValve == 2)
        {
            return "<color=red>Press [E]</color=red> to turn the valve";
        }
        else
        {
            return "Prepare the Compound";
        }
    }

    public override void Interact()
    {
        if (!isComplete && tertiaryBoolForValve == 2)
        {
            isComplete = true;
            valveCollider.enabled = false;
            animation.SetTrigger("activate");
            PlaySound();
            sparkle.SetActive(false);
            prepareVial.createVial();
        }   
            
    }



    private void PlaySound()
    {
        if (audioSource && valveSounds != null && valveSounds.Length > 0)
        {
            AudioClip randomClip = valveSounds[Random.Range(0, valveSounds.Length)];
            audioSource.clip = randomClip;
            audioSource.Play();
        }
    }

    IEnumerator closeVat()
    {
        prepareVial.closeVat(isComplete);
        yield return new WaitForSeconds(12f);
        tertiaryBoolForValve = 2;

    }
}
