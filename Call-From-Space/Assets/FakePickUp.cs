using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class FakePickUp : Interactable
{

    public string fakeItemName;
    public GameObject ItemGlow;
    public AudioClip PickUpSound;
    public AudioSource audioSource;
    public GameObject blackSmokeParticlesPrefab;
    Rigidbody playerRB;

    override protected void Awake()
    {
        base.Awake();
        //ItemGlow = Instantiate(ItemGlow, transform, true);
        //ItemGlow.transform.parent = transform;
        //ItemGlow.transform.position = this.transform.position;
    }
    public override void Interact()
    {
        Debug.Log("Activate");
        GameObject blackSmoke = Instantiate(blackSmokeParticlesPrefab, transform.position, Quaternion.identity);
        ItemGlow.SetActive(false);
        InsanityMeter.Instance.IncreaseInsanity(50f);
        Destroy(gameObject);
    }

    public override string GetDescription()
    {
        return ("Press [E] to pick up " + fakeItemName);
    }

}
