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

    public GameObject[] lightCones_and_light_objects;
    public Light[] lights;
    public Color poweredOnLightColor;

    [Header("Animator")]
    public Animator _WarpDriveAnimator;

    [Header("Sparkle")]
    public GameObject sparkle;

    [Header("Audios")]
    public AudioSource engineLoop;
    public AudioSource WarpDrive;

    [Header("AI Sounds")]
    public AI_Tutorial_Sounds AI_Sounds;
    // Start is called before the first frame update
    void Start()
    {
        engineLoop.enabled = true;
        WarpDrive.enabled = true;
        engineLoop.Pause();
        WarpDrive.Pause();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override string GetDescription()
    {
        if(!finished && !Open)
        {
            return ("Restabalize Pressure Valves First");
        }
        else if (Open && !finished)
            return ("<color=red>Press [E]</color=red>to Plug in Damaged Warp Drive");
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

        _WarpDriveAnimator.SetTrigger("Plugged");

        StartCoroutine(turnOnLights());
        StartCoroutine(PauseBeforeEnginePlay());
    }

    IEnumerator PauseBeforeEnginePlay()
    {
        WarpDrive.Play();
        yield return new WaitForSeconds(6f);
        engineLoop.Play();
        yield return null;

    }

    IEnumerator turnOnLights()
    {
        yield return new WaitForSeconds(1f);
        AI_Sounds.PlayEndLockDown();
        sparkle.SetActive(false);
        foreach(GameObject obj in lightCones_and_light_objects)
        {
            obj.SetActive(true);
        }
        foreach(Light light in lights)
        {
            light.color = poweredOnLightColor;
        }
        yield return null;

    }
}
