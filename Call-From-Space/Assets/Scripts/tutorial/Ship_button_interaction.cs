using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_button_interaction : Interactable
{

    public string Specialty_button_text;
    public bool off_until_special;
    public Ship_door_and_button_controller door_controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle_specialty()
    {
        off_until_special = false;
    }

    public override string GetDescription()
    {
        if (off_until_special)
        {
            return (Specialty_button_text);
        }
        else if (!door_controller.DoorIsOpen){
            return ("<color=red>Press [E]</color=red> to open the door.");
        }
        else{
            return ("<color=red>Press [E]</color=red> to close the door.");
        }
        return ("");
    }

    public override void Interact()
    {
        if (!off_until_special)
        {
           door_controller.ToggleDoor();
        }
    }
}
