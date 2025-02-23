using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveDeposit : Interactable
{

    public GameObject holdableValve;
    public GameObject player;
    public GameObject sparkle;
    public GameObject fixedValve;
    public AudioSource ValvePlugin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override string GetDescription()
    {
        if (player.GetComponent<Interactor>().holdingName == "Busted Valve")
            return "Press [E] to Insert Busted Valve";
        return "Find Busted Valve";
    }

    public override void Interact()
    {
        if (player.GetComponent<Interactor>().holdingName == "Busted Valve")
        {
            holdableValve.GetComponent<ValveHoldable>().DropObject();
            holdableValve.GetComponent<ValveHoldable>().StopGlowEffect();
            holdableValve.SetActive(false);
            sparkle.SetActive(false);
            fixedValve.SetActive(true);
            ValvePlugin.Play();
            
        }
    }
}
