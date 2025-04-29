using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLightSpin : MonoBehaviour
{
    public float rotationSpeed = 10f; // degrees per second
    private float currentYRotation = 0f;


    void Start()
    {
        // Store initial X and Z rotation angles
    }

    void Update()
    {
        currentYRotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentYRotation, 0f, 0f);
    }
}
