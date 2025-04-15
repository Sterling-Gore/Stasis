using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class labScreenInteraction : Interactable
{
    public GameObject LabUI;
    public GameObject player;
    public bool finished = false;
    public GameObject sparkle;
    public lab_valve_interaction valve;
    // Start is called before the first frame update


    public override string GetDescription()
    {
        if(!finished)
            return ("<color=red>Press [E]</color=red> to interact with the Lab Screen");
        return("");
    }

    public override void Interact()
    {
        if(!finished)
        {
            LabUI.SetActive(true);
            player.GetComponent<Interactor>().inUI = true;
            player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.inventory_or_puzzle);
        }
    }

    public void finishPuzzle()
    {
        finished = true;
        sparkle.SetActive(false);
        valve.puzzleReady = true;
    }
}
