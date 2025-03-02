using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_button_interaction : Interactable
{

    public enum SpeicalButtonType
    {
        NeedsSuit,
        NeedsPower,
        NeedsRadio,
        WrongWay,
        NeedsOxygen
    }

    public SpeicalButtonType SpecialType;

    public string Specialty_button_text;
    public bool off_until_special;
    public Ship_door_and_button_controller door_controller;
    public bool isEngineButton = false;
    public bool buttonUnlocksBothButtons = false;

    public bool HelpTextFlashlight = false;
    public bool HelpTextCrouch = false;
    public HelpTexts helptexts;


    public OxygenSystem oxygenSystem;

    [Header("AI Sounds")]
    public AI_Tutorial_Sounds AI_Sounds;



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
        if (isEngineButton && oxygenSystem.oxygenLevel >= 50f)
        {
            off_until_special = false;
        }

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
            if(isEngineButton)
            {
                AI_Sounds.PlayInitiateLockDown();
            }
           door_controller.ToggleDoor(isEngineButton, buttonUnlocksBothButtons);
           if(HelpTextFlashlight)
           {
                HelpTextFlashlight = false;
                helptexts.PressF = true;
           }
           if(HelpTextCrouch)
           {
                HelpTextCrouch = false;
                helptexts.PressCTRL = true;
           }
        }
        else
        {
            switch(SpecialType)
            {
                case SpeicalButtonType.NeedsSuit:
                    AI_Sounds.PlayINeedMySuit();
                    break;
                case SpeicalButtonType.WrongWay:
                    AI_Sounds.PlayWrongWay();
                    break;
                case SpeicalButtonType.NeedsPower:
                    AI_Sounds.PlayINeedPower();
                    break;
                case SpeicalButtonType.NeedsRadio:
                    AI_Sounds.PlayINeedRadio();
                    break;
                case SpeicalButtonType.NeedsOxygen:
                    AI_Sounds.PlayRefillOxygen();
                    break;
                default:
                    break;
            }
        }
    }
}
