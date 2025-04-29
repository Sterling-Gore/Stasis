using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaGun : Holdable
{

    public GameObject Laser;
    public AudioSource audioSource;
    public bool done = false;
    public bool grabbed = false;
    public AudioSource AI_Gun_Acquired;
    [Header("Camera Shake")]
    public CameraShakeGeneral cameraShake;

    public override string GetDescription()
    {
        if(done)
            return ("Out of power");
        return ("Press [E] to grab the " + objName);
    }

    public override void Interact()
    {
        if(!done)
        {
            PickUpObject();
            if(!grabbed)
            {
                grabbed = true;
                AI_Gun_Acquired.Play();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       if(gameObject.activeSelf && ItemGlow.activeSelf && !done)
            ItemGlow.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if (localHold)
        {
            MoveObject();
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                StopClipping();
                DropObject();
            }
        }
        if (player.GetComponent<Interactor>().holdingName == objName &&  Input.GetKey(KeyCode.Mouse0))
        {
            cameraShake.StartShake(0.02f, 0.1f);
            Laser.SetActive(true);
            audioSource.enabled = true;
            Ray ray = new Ray(transform.position, transform.right);
            //Ray ray = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.right * 10f, Color.green);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                //Debug.Log("YERRRRRR");
                BurningSpecimen burnSpecimen = hit.collider.GetComponent<BurningSpecimen>();
                if(burnSpecimen != null)
                {
                    Debug.Log("FOUND");
                    burnSpecimen.burnPlant();
                }
            }
        }
        else
        {
            Laser.SetActive(false);
            audioSource.enabled = false;
        } 
    }
}
