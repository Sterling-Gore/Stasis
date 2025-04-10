using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class FakePickUp : Interactable
{

    public string fakeItemName;
    public Transform player;
    public GameObject ItemGlow;
    public AudioClip PickUpSound;
    public AudioSource audioSource;
    public Transform teleportPoint;
    public CameraController cameraController;
    public Camera playerCamera;
    public Vector3 originalPlayerPosition;
    public Vector3 originalPlayerRotation;

    Rigidbody playerRB;
    override protected void Awake()
    {
        base.Awake();
        ItemGlow = Instantiate(ItemGlow, transform, true);
        ItemGlow.transform.position = this.transform.position;
        playerRB = player.gameObject.GetComponent<Rigidbody>();
    }
    public override void Interact()
    {
        Debug.Log("Activate");
        originalPlayerPosition = playerRB.position;
        originalPlayerRotation = playerCamera.transform.localEulerAngles;
        originalPlayerRotation.x = 0;
        playerRB.position = teleportPoint.position;
        cameraController.AlignRotation(teleportPoint.localEulerAngles);
        transform.parent.gameObject.SetActive(false);
    }

    public override string GetDescription()
    {
        return ("Press [E] to pick up " + fakeItemName);
    }
}
