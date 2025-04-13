using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBack : Interactable
{
    public FakePickUp originPickUp;
    Rigidbody playerRB;
    HealthSystem healthSystem;
    
    public GameObject itemGlow;

    int caughtCount;
    int timesInteracted;

    public Light spotlight;

    public string[] descriptions;
    public override string GetDescription()
    {
        return descriptions[timesInteracted];
    }

    public override void Interact()
    {
        float damageTaken = ((timesInteracted+1) / 5f) * healthSystem.healthLevel;

        spotlight.spotAngle -= 7;
        
        healthSystem.TakeDamage(damageTaken);
        //playerRB.position = originPickUp.originalPlayerPosition;
        //playerRB.isKinematic = true;
        //playerRB.isKinematic = false;
        //originPickUp.cameraController.AlignRotation(originPickUp.originalPlayerRotation);
        timesInteracted++;
    }

    private void Awake()
    {
        timesInteracted = 0;
        caughtCount = 5;

        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        healthSystem = playerRB.GetComponent<HealthSystem>();
        itemGlow = Instantiate(itemGlow, transform, true);
        itemGlow.transform.position = this.transform.position;
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

        
    //}
}
