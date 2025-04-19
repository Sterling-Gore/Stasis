using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plant_deposit : Interactable
{
    // Start is called before the first frame update
    public GameObject holdablePlant;
    public GameObject player;
    public GameObject sparkle;
    public GameObject plant;
    public AudioSource placeDownAudio;
    public lab_valve_interaction valve;


    public override string GetDescription()
    {
        if (player.GetComponent<Interactor>().holdingName == "Foreign Plant")
            return "<color=red>Press [E]</color=red> to Insert Foreign Plant";
        return "Find Foreign Plant";
    }

    public override void Interact()
    {
        if (player.GetComponent<Interactor>().holdingName == "Foreign Plant")
        {
            holdablePlant.GetComponent<ValveHoldable>().DropObject();
            holdablePlant.GetComponent<ValveHoldable>().StopGlowEffect();
            holdablePlant.SetActive(false);
            if(sparkle != null)
                sparkle.SetActive(false);
            plant.SetActive(true);
            placeDownAudio.Play();
            valve.holdableReady = true;

            gameObject.SetActive(false);
            
        }
    }
}
