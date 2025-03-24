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
        }
    }
}
