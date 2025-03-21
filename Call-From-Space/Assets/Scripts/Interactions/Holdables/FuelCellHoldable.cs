using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCellHoldable : Holdable
{
    public AudioSource audioSource;
    public Vector3 DepositRotation;
    public Vector3 DepositPosition;

    void Update()
    {
        if(gameObject.activeSelf && ItemGlow.activeSelf)
            ItemGlow.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if(localHold) 
        {
            MoveObject();
            if (Input.GetKeyDown(KeyCode.Mouse1) ) 
            {
                StopClipping();
                DropObject();
                audioSource.Play();
            }
        }
    }

    public void deposit()
    {
        DropObject();
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Holdable>().enabled = false;
        transform.position = DepositPosition;
        transform.rotation = Quaternion.Euler(DepositRotation.x, DepositRotation.y, DepositRotation.z);
        StopGlowEffect();
        gameObject.GetComponent<Collider>().enabled = false;
    }


}
