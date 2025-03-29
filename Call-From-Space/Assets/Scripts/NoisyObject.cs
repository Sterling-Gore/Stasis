using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoisyObject : MonoBehaviour
{
    // Start is called before the first frame update
    public int noiseValue;

    float tickTimer = 0;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        float playerVelocity = other.GetComponentInParent<Rigidbody>().velocity.magnitude;
        if (playerVelocity == 0) return;


        if (tickTimer < 2 / playerVelocity)
            AlienAttentionHandler.NoiseToAttentionIncrease(noiseValue, transform.position);
        else
            tickTimer += Time.deltaTime;
    }

}
