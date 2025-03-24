using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDoors_Workshop :  Interactable
{
    public bool poweredOn = false;
    public string doorText = "This Door Needs Power";

    [Header("Animation")]
    public Animator doorAnimation;

    [Header("Special")]
    public bool startsOpen;
    public bool startsBroken;
    // Start is called before the first frame update
    void Start()
    {
        if(startsOpen)
        {
            PowerOn();
        }
        else if(startsBroken)
        {
            breakDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PowerOn()
    {
        poweredOn = true;
        doorAnimation.SetTrigger("open");
    }

    public void PowerOff()
    {
        poweredOn = false;
        doorAnimation.SetTrigger("close");
    }

    public void breakDoor()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        doorAnimation.SetTrigger("brokenDoor");
        poweredOn = true;
    }

    public override string GetDescription()
    {
        if(!poweredOn)
            return (doorText);
        return ("");
    }

    public override void Interact()
    {
        if(!poweredOn)
        {
            //play audio clip
        }
    }
}
