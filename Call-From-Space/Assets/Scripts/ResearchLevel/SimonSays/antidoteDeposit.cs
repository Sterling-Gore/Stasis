using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antidoteDeposit : Interactable
{
    // Start is called before the first frame update
    public GameObject holdableAntidote;
    public GameObject player;
    public GameObject antidoteDepositSparkle;
    public GameObject simonSaysSparkles;
    public AudioSource placeDownAudio;
    public SimonSaysScreemInteraction SimonSaysScreen;
   
    public override string GetDescription()
    {
        if (player.GetComponent<Interactor>().holdingName == "Mixed Antidote Compound")
            return "<color=red>Press [E]</color=red> to Insert Mixed Antidote Compound";
        return "Find Mixed Chemical Compound";
    }

    public override void Interact()
    {
        if (player.GetComponent<Interactor>().holdingName == "Mixed Antidote Compound")
        {
            holdableAntidote.GetComponent<ValveHoldable>().DropObject();
            holdableAntidote.GetComponent<ValveHoldable>().StopGlowEffect();
            holdableAntidote.SetActive(false);
            if(antidoteDepositSparkle != null)
                antidoteDepositSparkle.SetActive(false);
            if(simonSaysSparkles != null)
                simonSaysSparkles.SetActive(true);
            placeDownAudio.Play();
            SimonSaysScreen.antidotePrepared = true;

            gameObject.SetActive(false);
            
        }
    }
}
