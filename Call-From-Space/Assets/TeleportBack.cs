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
        shadowRealmController.ItemInteraction();      
    }

    private void Awake()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        shadowRealmController = FindObjectOfType<ShadowRealm>();
        cameraController = FindObjectOfType<CameraController>();
        healthSystem = playerRB.GetComponent<HealthSystem>();
        itemGlow = Instantiate(itemGlow, transform, true);
        itemGlow.transform.position = this.transform.position;
        

    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!other.CompareTag("Player")) return;

        
    //}
}
