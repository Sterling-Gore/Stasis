using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spacesuit_Pickup : Interactable
{
    public UI_Controller ui_controller;
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
        return ("<color=red>Press [E]</color=red> to put on spacesuit");
    }

    public override void Interact()
    {
        ui_controller.PutOnSuit();
        gameObject.SetActive(false);
    }
}
