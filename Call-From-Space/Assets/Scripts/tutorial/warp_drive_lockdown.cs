using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp_drive_lockdown : Interactable
{
    public Ship_button_interaction EngineButton1;
    public Ship_button_interaction EngineButton2;
    public Ship_button_interaction LeftHallwayButton;
    public Ship_button_interaction RightHallwayButton;

    public Ship_door_and_button_controller LeftHallwayDoor;
    public Ship_door_and_button_controller RightHallwayDoor;
    public Ship_door_and_button_controller EngineDoor;
    public ConsoleInteraction consoleInteractable;

    public OxygenSystem oxygenSystem; 
    public bool Open = false;
    bool finished = false;
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
        if (Open && !finished)
            return ("Plug in Damaged Warp Drive");
        return ("");
    }

    public override void Interact()
    {
        if (Open && !finished)
        {
            End_Lockdown();
            finished = true;
            consoleInteractable.IsAvailable = true;
        }
    }



    public void Start_Lockdown()
    {
        if (LeftHallwayDoor.DoorIsOpen)
            LeftHallwayDoor.ToggleDoor(false, false);
        if (RightHallwayDoor.DoorIsOpen)
            RightHallwayDoor.ToggleDoor(false, false);
        
        LeftHallwayButton.off_until_special = true;
        RightHallwayButton.off_until_special = true;
        EngineButton1.off_until_special = true;
        EngineButton2.off_until_special = true;

        LeftHallwayButton.Specialty_button_text = "Not Available During Lockdown";
        RightHallwayButton.Specialty_button_text = "Not Available During Lockdown";
        EngineButton1.Specialty_button_text = "Not Available During Lockdown";
        EngineButton2.Specialty_button_text = "Not Available During Lockdown";

        oxygenSystem.LosingOxygen = true;
    }

    public void End_Lockdown()
    {
        LeftHallwayButton.off_until_special = false;
        RightHallwayButton.off_until_special = false;
        EngineButton1.off_until_special = false;
        EngineButton2.off_until_special = false;
        EngineDoor.ToggleDoor(false, false);
    }
}
