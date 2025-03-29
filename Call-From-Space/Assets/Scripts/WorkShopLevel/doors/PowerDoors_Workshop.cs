using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PowerDoors_Workshop :  Interactable
{
    public bool poweredOn = false;
    public string doorText = "This Door Needs Power";

    [Header("Animation")]
    public Animator doorAnimation;

    [Header("Special")]
    public bool startsOpen;
    public bool startsBroken;

    public event EventHandler<DoorEventArgs> DoorActivated;
    [SerializeField]
    GameObject[] associatedNodes; 

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
        OnDoorActivated(new DoorEventArgs(associatedNodes));
        
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
        OnDoorActivated(new DoorEventArgs(associatedNodes));
        
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

    protected virtual void OnDoorActivated(DoorEventArgs e)
    {
        DoorActivated?.Invoke(this, e);
    }
}

public class DoorEventArgs : EventArgs
{
    public GameObject[] associatedNodes;
    public DoorEventArgs(GameObject[] nodes)
    {
        associatedNodes = nodes;
    }
}