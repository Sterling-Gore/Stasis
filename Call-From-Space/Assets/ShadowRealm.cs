using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class ShadowRealm : MonoBehaviour
{
    [SerializeField] Rigidbody playerRB;
    [SerializeField] Transform teleportPoint;
    [SerializeField] Light spotlight;

    [NonSerialized]
    public Vector3 originalPlayerPosition, originalPlayerRotation;
    int timesInteracted;

    HealthSystem healthSystem;
    CubeEyeRotate eyeRotator;
    AudioSource scaryAmbient;
    CameraController cameraController;
    Camera playerCamera;
    DarkFigureController darkFigureController;

    float initialSpotangle;
    

    // Start is called before the first frame update
    void Start()
    {
        scaryAmbient = GetComponentInChildren<AudioSource>();
        playerCamera = FindObjectOfType<Camera>();
        cameraController = FindObjectOfType<CameraController>();
        healthSystem = playerRB.GetComponent<HealthSystem>();
        eyeRotator = GetComponentInChildren<CubeEyeRotate>();
        darkFigureController = FindObjectOfType<DarkFigureController>();

        timesInteracted = 0;
        initialSpotangle = spotlight.spotAngle;
        eyeRotator.enabled = false;
    }

    // Update is called once per frame

    public void TeleportToShadowRealm()
    {
        Debug.Log("Activate");

        scaryAmbient.Play();
        InsanityMeter.Instance.acceptingInsanityIncrease = false;
        eyeRotator.enabled = true;
        originalPlayerPosition = playerRB.position;
        originalPlayerRotation = playerCamera.transform.localEulerAngles;
        originalPlayerRotation.x = 0;
        playerRB.position = teleportPoint.position;
        cameraController.AlignRotation(teleportPoint.localEulerAngles);
    }

    public void ItemInteraction()
    {
        float damageTaken = ((++timesInteracted) / (float)InsanityMeter.Instance.maxCaught) * healthSystem.healthLevel;
        healthSystem.TakeDamage(damageTaken);

        Debug.Log(InsanityMeter.Instance.timesCaught + " : " + timesInteracted);
        spotlight.spotAngle -= 10;
        if (timesInteracted == InsanityMeter.Instance.timesCaught)
        {
            TeleportToPreviousPosition();
        }
    }

    public void TeleportToPreviousPosition()
    {
        darkFigureController.SendToTimeout(30f);
        scaryAmbient.Stop();
        spotlight.spotAngle = initialSpotangle;
        InsanityMeter.Instance.acceptingInsanityIncrease = true;
        playerRB.position = originalPlayerPosition;
        playerRB.isKinematic = true;
        playerRB.isKinematic = false;
        cameraController.AlignRotation(originalPlayerRotation);
        timesInteracted = 0;
    }
}
