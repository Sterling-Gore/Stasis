using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationPanel :  Interactable
{
    // Start is called before the first frame update
    public SaveManager saveManager;
    public PowerDoors_Workshop FrontRoomDoor;
    public bool active = true;
    public GameObject sparkle;

    public PowerDoors_Workshop EntryDoor;
    
    [Header("Map")]
    public MapManager mapManager;
    public GameObject workshopMap;

    [Header("Oxygen System")]
    public OxygenSystem oxygenSystem;

    [Header("Help Texts")]
    public HelpTexts helpText;

    [Header("Audios")]
    public AudioSource DoorSound1;
    public AudioSource DoorSound2;

    void Start()
    {
        if(!active)
        {
            sparkle.SetActive(false);
        }
    } 
    public override string GetDescription()
    {
        if(active)
            return ("<color=red>Press [E]</color=red> to Authenticate Yourself in the Stasis System");
        return ("");
    }

    public override void Interact()
    {
        if(active)
        {
            active = false;
            FrontRoomDoor.PowerOn();
            saveManager.UpdateSave(SavePointID.workshop2);
            sparkle.SetActive(false);
            EntryDoor.PowerOff();
            DoorSound1.Play();
            DoorSound2.Play();

            mapManager.SetMap(workshopMap);
            oxygenSystem.LosingOxygen = true;
            helpText.NewMapAdded = true;
        }
    }
}
