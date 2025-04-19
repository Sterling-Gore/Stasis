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


    public override string GetDescription()
    {
        if (isComplete)
        {
            return "";
        }
        if(puzzleReady && holdableReady)
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
        if (!isComplete && puzzleReady && holdableReady)
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
}
