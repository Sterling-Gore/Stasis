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

    [Header("Dialogue")]
    public AudioSource StasisAI_lockdown;
    public AudioSource AI_OxygenOffline;
    public AudioSource AI_Override;

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
            EntryDoor.PowerOff();
            sparkle.SetActive(false);
            DoorSound1.Play();
            oxygenSystem.LosingOxygen = true;

            StartCoroutine(startLockdown());
        }
    }

    IEnumerator startLockdown()
    {
        StasisAI_lockdown.Play();
        yield return new WaitForSeconds(6f);
        AI_OxygenOffline.Play();
        yield return new WaitForSeconds(4f);
        AI_Override.Play();
        yield return new WaitForSeconds(6f);
        mapManager.SetMap(workshopMap);
        helpText.NewMapAdded = true;
        FrontRoomDoor.PowerOn();
        DoorSound2.Play();
        saveManager.UpdateSave(SavePointID.workshop2);
    }
}
