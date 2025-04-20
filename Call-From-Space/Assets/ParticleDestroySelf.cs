using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroySelf : MonoBehaviour
{
    ParticleSystem blackSmokeParticles;
    void Start()
    {
        blackSmokeParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!blackSmokeParticles.isPlaying)
            Destroy(gameObject);
    }
}
