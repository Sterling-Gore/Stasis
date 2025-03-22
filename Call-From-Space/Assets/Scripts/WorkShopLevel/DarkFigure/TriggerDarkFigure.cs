using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDarkFigure : MonoBehaviour
{
    public bool isStartingTrigger;
    public ManagerDarkFigure managerDarkFigure;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(isStartingTrigger)
                managerDarkFigure.spawnFigure();
            else
                managerDarkFigure.despawnFigure();
            gameObject.SetActive(false);
        }
    }
}
