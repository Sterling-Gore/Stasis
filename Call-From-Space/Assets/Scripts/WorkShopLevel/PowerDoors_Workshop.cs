using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDoors_Workshop :  Interactable
{
    bool poweredOn = false;
    public Animator doorAnimation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PowerOn()
    {
        poweredOn = true;
        //doorAnimation.setTrigger("open");
    }

    public override string GetDescription()
    {
        if(!poweredOn)
            return ("This door needs power");
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
