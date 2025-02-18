using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_door_and_button_controller : MonoBehaviour
{
    public bool DoorIsOpen;
    //public Material myMaterial;
    //public Texture redTexture;
    //public Texture greenTexture;
    public Material greenMaterial;
    public Material redMaterial;

    public Animator _doorAnimator;

    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip doorOpenSound;

    public Ship_button_interaction button1;
    public Ship_button_interaction button2;
    public Renderer button1Renderer;
    public Renderer button2Renderer;

    // Start is called before the first frame update
    void Start()
    {
        button1Renderer.material.EnableKeyword ("ON");
        button1Renderer.material.EnableKeyword ("OFF");
        button2Renderer.material.EnableKeyword ("ON");
        button2Renderer.material.EnableKeyword ("OFF");
        
        //if (DoorIsOpen)
        //{
        //    button1Renderer.material.SetTexture("ON", greenTexture);
        //    button2Renderer.material.SetTexture("ON", greenTexture);
        //}
        //else
        //{
        //    button1Renderer.material.SetTexture("OFF", redTexture);
        //    button2Renderer.material.SetTexture("OFF", redTexture);
        //}
        // _doorAnimator.SetTrigger(DoorIsOpen ? "open" : "closed");
        button1Renderer.material =  DoorIsOpen ? greenMaterial : redMaterial;
        button2Renderer.material =  DoorIsOpen ? greenMaterial : redMaterial;

        

   
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
            if (!audioSource)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        audioSource.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ToggleDoor()
    {
        DoorIsOpen = !DoorIsOpen;
        //if (DoorIsOpen)
        //{
        //    button1Renderer.material.SetTexture("ON", greenTexture);
        //    button2Renderer.material.SetTexture("ON", greenTexture);
        //}
        //else
        //{
        //    button1Renderer.material.SetTexture("OFF", redTexture);
        //    button2Renderer.material.SetTexture("OFF", redTexture);
        //}
        button1Renderer.material =  DoorIsOpen ? greenMaterial : redMaterial;
        button2Renderer.material =  DoorIsOpen ? greenMaterial : redMaterial;
        _doorAnimator.SetTrigger(DoorIsOpen ? "open" : "closed");

        PlaySound(buttonClickSound);
            
        StartCoroutine(PlayDelayedSound(doorOpenSound, 0.5f));

        button1.off_until_special = false;
        button2.off_until_special = false;
    }


    private void PlaySound(AudioClip clip)
    {
        if (audioSource  && clip )
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }
    
    private System.Collections.IEnumerator PlayDelayedSound(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(clip);
    }
}
