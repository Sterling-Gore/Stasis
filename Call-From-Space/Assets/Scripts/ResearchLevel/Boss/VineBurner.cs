using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class VineBurner : MonoBehaviour
{
    private MaterialPropertyBlock propBlock;
    public Renderer rend;
    private float burnAmount = 0f;

    public float burnSpeed = 0.5f;

    public ParticleSystem electricParticles;
    private ParticleSystem.MainModule lightningMain;
    private ParticleSystem.EmissionModule lightningEmission;

    public bool burn = false;

    public GameObject vine;

    void Start()
    {
        //rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        lightningMain = electricParticles.main;
        lightningEmission = electricParticles.emission;
    }

    void Update()
    {
        if (burn && burnAmount < 1f)
        {
            burnAmount += Time.deltaTime * burnSpeed;

            // Get the current property block
            rend.GetPropertyBlock(propBlock);

            // Set the burn amount individually
            propBlock.SetFloat("_BurnThreshold", burnAmount);

            // Apply it back
            rend.SetPropertyBlock(propBlock);

            lightningMain.simulationSpeed = Mathf.Lerp(1.0f, 0.2f, burnAmount);
            var emissionRate = Mathf.Lerp(50.0f, 0.0f, burnAmount);
            lightningEmission.rateOverTime = emissionRate;
        }
        else if (burn && burnAmount >= 1f)
        {
            burn = false;
            electricParticles.Stop();
            vine.SetActive(false);

        }
    }

    public void startBurn()
    {
        burn = true;
        electricParticles.Play();
    }
}