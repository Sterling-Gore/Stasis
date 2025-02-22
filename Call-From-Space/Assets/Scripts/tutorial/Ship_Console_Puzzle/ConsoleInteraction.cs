using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInteraction : Interactable
{

    public GameObject ConsoleUI;
    public GameObject player;
    public bool IsAvailable;
    public bool Finished;

    // Start is called before the first frame update
    void Start()
    {
        IsAvailable = false;
        Finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string GetDescription()
    {
        if (Finished)
            return ("<color=red>Press [E]</color=red> to Start Your Ship");
        else if(IsAvailable)
            return ("<color=red>Press [E]</color=red> to Access the Flightdeck Console");
        return ("Flightdeck Console is Offline, Restore Power to Engine");
    }

    public override void Interact()
    {
        if (Finished){}
        else if(IsAvailable)
        {
            ConsoleUI.SetActive(true);
            player.GetComponent<Interactor>().inUI = true;
            player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.inventory_or_puzzle);
        }
    }
}
