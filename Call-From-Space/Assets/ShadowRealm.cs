using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShadowRealm : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform teleportPoint;

    CameraController cameraController;
    Camera playerCamera;

    [NonSerialized]
    public Vector3 originalPlayerPosition, originalPlayerRotation;
    // Start is called before the first frame update
    void Start()
    {

        playerCamera = FindObjectOfType<Camera>();
        cameraController = FindObjectOfType<CameraController>();
    }

    // Update is called once per frame

    public void TeleportToShadowRealm()
    {
        Debug.Log("Activate");

        InsanityMeter.Instance.acceptingInsanityIncrease = false;

        originalPlayerPosition = playerRB.position;
        originalPlayerRotation = playerCamera.transform.localEulerAngles;
        originalPlayerRotation.x = 0;
        playerRB.position = teleportPoint.position;
        cameraController.AlignRotation(teleportPoint.localEulerAngles);
    }
}
