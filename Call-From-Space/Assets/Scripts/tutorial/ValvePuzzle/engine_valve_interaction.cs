using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class engine_valve_interaction : Interactable
{
    public EngineValveManager engineValveManager;
    private Animator animation;
    public AudioSource audioSource;
    private Collider valveCollider;
    public AudioClip[] valveSounds;
    public GameObject smoke;

    public bool isBroken = false;
    public bool isOn = false;
    public bool isComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>();
        valveCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public override string GetDescription()
    {
        if (isComplete)
        {
            return "";
        }
        return "Press [E] to turn the valve";
    }

    public override void Interact()
    {
        if (!isComplete)   
            StartCoroutine(InteractSequence());
    }


    private IEnumerator InteractSequence()
    {
        valveCollider.enabled = false;
        isOn = !isOn;
        animation.SetTrigger(isOn ? "On" : "Off");
        PlaySound();
        smoke.SetActive( (!isOn && isBroken) || (isOn && !isBroken));
        engineValveManager.UpdateValve();
        yield return new WaitForSeconds(0.2f);
        valveCollider.enabled = true;


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
