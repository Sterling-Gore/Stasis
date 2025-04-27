using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysScreemInteraction : Interactable
{
    public GameObject screenUI;
    public GameObject player;
    public bool finished = false;
    public bool antidotePrepared = false;
    public GameObject sparkle;

    [Header("SimonSaysManger")]
    public SimonSaysManager manager;
    // Start is called before the first frame update
  
    public override string GetDescription()
    {
        if(!antidotePrepared)
            return ("Insert Mixed Chemical Compound First");
        else if(!finished)
            return ("<color=red>Press [E]</color=red> to interact with the Screen");

        return("");
    }

    public override void Interact()
    {
        if(!finished && antidotePrepared)
        {
            screenUI.SetActive(true);
            player.GetComponent<Interactor>().inUI = true;
            player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.inventory_or_puzzle);
        }
    }

    public void finishPuzzle()
    {
        finished = true;
        sparkle.SetActive(false);
        manager.completePuzzle();
    }
}
