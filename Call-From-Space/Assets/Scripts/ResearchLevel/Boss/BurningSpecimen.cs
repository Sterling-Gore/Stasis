using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningSpecimen : MonoBehaviour
{
    
    public bool doneBurning;
    public ParticleSystem[] Electricities;
    public GameObject[] plantObjects;
    public Material[] charredPlants;
    public AudioSource FinalScreech;

    public bool isBurning = false;
    public bool burnChecker = false;
    public float burnSpeed = 0.005f;
    public float burnAmount = 0f;
    Coroutine burnCoroutine;

    public SaveManager saveManager;
    public PlasmaGun gun;

    void Start()
    {
        doneBurning = false;
        foreach( Material mat in charredPlants)
        {
            mat.SetFloat("_BurnThreshold", 0f);
        }
    }

    void Update()
    {
        if(burnAmount > .5f && !doneBurning)
        {
            completeBurning();
            turnOffElectricity();
        }
        if(isBurning != burnChecker)
        {
            if(isBurning)
            {
                turnOnElectricity();
                burnChecker = true;
            }
            else
            {
                turnOffElectricity();
                burnChecker = false;
            }
        }

        if(isBurning)
        {
            if (burnAmount < .5f)
            {
                burnAmount += Time.deltaTime * burnSpeed;
                foreach( Material mat in charredPlants)
                {
                    mat.SetFloat("_BurnThreshold", burnAmount);
                }
                //vineMaterial.SetFloat("_BurnThreshold", burnAmount);
            }
            else
            {
                completeBurning();
            }
        }
    }

    public void burnPlant()
    {
        if(!doneBurning && burnCoroutine == null)
            burnCoroutine = StartCoroutine(burn());
    }

    public void completeBurning()
    {
        doneBurning = true;
        isBurning = false;
        gun.done = true;
        gun.ItemGlow.transform.position = new Vector3(0f,0f,0f);
        saveManager.UpdateSave(SavePointID.research5);
        StartCoroutine(AudioOff());
        StartCoroutine(FadeAway());
    }

    public void turnOnElectricity()
    {
        foreach(ParticleSystem electricity in Electricities)
        {
            electricity.Play();
        }
    }

    public void turnOffElectricity()
    {
        foreach(ParticleSystem electricity in Electricities)
        {
            electricity.Stop();
        }
    }

    IEnumerator burn()
    {
        float time = 1f;
        while(time > 0f && !doneBurning)
        {
            isBurning = true;
            time -= Time.deltaTime;
            yield return null;

        }
        isBurning = false;
        burnCoroutine = null;
    }

    IEnumerator FadeAway()
    {
        while(burnAmount < 1f)
        {
            burnAmount += Time.deltaTime * burnSpeed;
            foreach( Material mat in charredPlants)
            {
                mat.SetFloat("_BurnThreshold", burnAmount);
            }
            yield return null;

        }
        foreach( GameObject plant in plantObjects)
        {
            plant.SetActive(false);
        }

    }

    IEnumerator AudioOff()
    {
        while(FinalScreech.volume > 0f)
        {
            FinalScreech.volume -= 0.1f * Time.deltaTime;
            yield return null;
        }
        FinalScreech.Stop();
    }
}
