using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeGeneral : MonoBehaviour
{
    [SerializeField] private float shakeDuration = 0.05f; // Duration of the shake
    [SerializeField] private float shakeMagnitude = 0.1f; // Magnitude of the shake
    public Transform CameraPosition;
    public Transform CameraHolderPosition;
    public bool isShaking = false;

    public MoveCamera movecamera;



    void Update()
    {
        if (isShaking)
        {
            // Apply random shake within a sphere of shakeMagnitude
            transform.localPosition = CameraPosition.position + Random.insideUnitSphere * shakeMagnitude;
        }

    }

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        movecamera.active = false;
        isShaking = true;
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
        transform.localPosition = CameraPosition.position; // Reset position after shaking
        movecamera.active = true;
    }
}
