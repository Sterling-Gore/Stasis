using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveHoldable : Holdable
{

    public AudioSource audioSource;
    public AudioClip PickUp;
    public AudioClip Drop;
    bool JustPickedUp = false;


    // Update is called once per frame
    void Update()
    {
        if(localHold && !JustPickedUp)
        {
            audioSource.PlayOneShot(PickUp);
        }

        if(gameObject.activeSelf && ItemGlow.activeSelf)
            ItemGlow.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if(localHold) 
        {
            MoveObject();
            JustPickedUp = true;
            if (Input.GetKeyDown(KeyCode.Mouse1) ) 
            {
                StopClipping();
                DropObject();
                audioSource.PlayOneShot(Drop);
                JustPickedUp = false;
                
            }
        }
    }
}
