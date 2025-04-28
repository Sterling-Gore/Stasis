using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHeadMover : MonoBehaviour
{
    /*
    public Transform player;
    public float rotationSpeed = 5f; 

    private Quaternion rotationOffset = Quaternion.FromToRotation(Vector3.forward, Vector3.left);

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // Ignore vertical difference

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= rotationOffset;

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    */


    public Transform player;
    public float rotationSpeed = 5f; // Adjust this in the Inspector

    public float rotationOffsetY = 90f; // Rotate 90 degrees on Y to fix "left" facing issue

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f; // Only rotate around the Y axis

        if (direction.sqrMagnitude > 0.001f) // Prevent zero direction issues
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(0f, rotationOffsetY, 0f); // Apply Y axis rotation correction

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
