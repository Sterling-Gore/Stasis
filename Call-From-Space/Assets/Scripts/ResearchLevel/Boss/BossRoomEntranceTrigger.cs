using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEntranceTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DoorCollider;
    public SpikeStabber spikeStabber;
    public AudioSource plantYell;
    public AudioSource AI_warns_plant;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spikeStabber.isActive = true;
            DoorCollider.SetActive(false);
            plantYell.Play();
            //AI_warns_plant.Play();
            StartCoroutine(delayAudio());
            //gameObject.SetActive(false);
            gameObject.GetComponent<Collider>().enabled = false;

        }
    }

    IEnumerator delayAudio()
    {
        yield return new WaitForSeconds(4f);
        AI_warns_plant.Play();
    }
}
