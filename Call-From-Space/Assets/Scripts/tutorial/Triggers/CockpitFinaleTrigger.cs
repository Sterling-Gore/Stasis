using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitFinaleTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Ship_button_interaction LeftHallwayButton;
    public Ship_button_interaction RightHallwayButton;
    public Ship_door_and_button_controller LeftHallwayDoor;
    public Ship_door_and_button_controller RightHallwayDoor;

    void OnTriggerEnter()
    {
        if (LeftHallwayDoor.DoorIsOpen)
            LeftHallwayDoor.ToggleDoor(false, false);
        if (RightHallwayDoor.DoorIsOpen)
            RightHallwayDoor.ToggleDoor(false, false);
        
        LeftHallwayButton.off_until_special = true;
        RightHallwayButton.off_until_special = true;
        LeftHallwayButton.Specialty_button_text = "Start the Ship";
        RightHallwayButton.Specialty_button_text = "Start the Ship";
        LeftHallwayButton.SpecialType = Ship_button_interaction.SpeicalButtonType.WrongWay;
        RightHallwayButton.SpecialType = Ship_button_interaction.SpeicalButtonType.WrongWay;

        gameObject.SetActive(false);
    }

}
