using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBack : Interactable
{
    ShadowRealm shadowRealmController;
    Rigidbody playerRB;
    HealthSystem healthSystem;
    CameraController cameraController;
    
    public GameObject itemGlow;

    public Light spotlight;

    public string[] descriptions;

    int timesInteracted;

    public override string GetDescription()
    {
        return descriptions[timesInteracted];
        
    }

    public override void Interact()
    {
        float damageTaken = ((++timesInteracted) /(float) InsanityMeter.Instance.maxCaught) * healthSystem.healthLevel;
        healthSystem.TakeDamage(damageTaken);

        Debug.Log(InsanityMeter.Instance.timesCaught + " : " + timesInteracted);
        spotlight.spotAngle -= 10;
        if(timesInteracted == InsanityMeter.Instance.timesCaught)
        {
            playerRB.position = shadowRealmController.originalPlayerPosition;
            playerRB.isKinematic = true;
            playerRB.isKinematic = false;
            cameraController.AlignRotation(shadowRealmController.originalPlayerRotation);
            timesInteracted = 0;
        }

            
    }

    private void Awake()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        shadowRealmController = FindObjectOfType<ShadowRealm>();
        cameraController = FindObjectOfType<CameraController>();
        healthSystem = playerRB.GetComponent<HealthSystem>();
        itemGlow = Instantiate(itemGlow, transform, true);
        itemGlow.transform.position = this.transform.position;
        timesInteracted = 0;

    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

        
    //}
}
