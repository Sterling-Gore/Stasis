using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEntranceTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DoorCollider;
    public SpikeStabber spikeStabber;
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
            gameObject.SetActive(false);

        }
    }
}
