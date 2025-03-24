using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BustedDoorTrigger : MonoBehaviour
{
    public PowerDoors_Workshop bustedDoor;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        if(!active)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            active = false;
            bustedDoor.breakDoor();
            gameObject.SetActive(false);
        }
    }
}
