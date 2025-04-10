using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBack : MonoBehaviour
{
    public FakePickUp originPickUp;
    Rigidbody playerRB;

    private void Awake()
    {
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerRB.position = originPickUp.originalPlayerPosition;
        playerRB.isKinematic = true;
        playerRB.isKinematic = false;
        originPickUp.cameraController.AlignRotation(originPickUp.originalPlayerRotation);
    }
}
