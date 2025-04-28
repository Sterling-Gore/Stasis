using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FakeItemSpawner : MonoBehaviour
{
    [SerializeField] 
    PickUp[] allPickupsToFake;
    [SerializeField]
    GameObject itemAura;
    //currently unused
    [SerializeField]
    AudioClip pickUpAudioClip;
    [SerializeField]
    GameObject blackSmokeParticlesPrefab;

    LOSChecker los;

    void Start()
    {
        los = FindObjectOfType<LOSChecker>();
        allPickupsToFake.ToList().ForEach(pickup => pickup.ItemPickedUp += ReadySpawn);
    }

    void ReadySpawn(object sender, PickUpEventArgs e)
    {
        Debug.Log("readying fake item");
        GameObject pickUpObject = e.pickUpItem;

        GameObject pickUpObjectCopy = Instantiate(pickUpObject, pickUpObject.transform.position, pickUpObject.transform.rotation);

        Item itemScript = pickUpObject.GetComponent<Item>();
        string originalItemName = itemScript.itemName;

        GameObject itemAuraInstance = Instantiate(itemAura);
        itemAuraInstance.transform.parent = pickUpObjectCopy.transform;
        itemAuraInstance.transform.position =pickUpObjectCopy.transform.position;

        Destroy(pickUpObjectCopy.GetComponent<PickUp>());
        Destroy(pickUpObjectCopy.GetComponent<Item>());

        FakePickUp fakeScript = pickUpObjectCopy.AddComponent<FakePickUp>();
        fakeScript.fakeItemName = originalItemName;
        fakeScript.blackSmokeParticlesPrefab = blackSmokeParticlesPrefab;
        fakeScript.ItemGlow = itemAuraInstance;

        pickUpObjectCopy.SetActive(false);
        StartCoroutine(WaitForOutOfSight(pickUpObjectCopy));
    }

    IEnumerator WaitForOutOfSight(GameObject fakeItem)
    {
        while (los.isOnScreen(fakeItem.transform.position))
        {
            yield return new WaitForFixedUpdate();
        }
        
        fakeItem.SetActive(true);
    }
}

