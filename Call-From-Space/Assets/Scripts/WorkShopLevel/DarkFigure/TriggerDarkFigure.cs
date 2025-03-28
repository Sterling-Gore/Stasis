using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDarkFigure : MonoBehaviour
{
    public bool isStartingTrigger;
    public ManagerDarkFigure managerDarkFigure;
    public bool UseImage = false;
    public bool UseAudio = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(isStartingTrigger)
                managerDarkFigure.spawnFigure(UseImage, UseAudio);
            else
                managerDarkFigure.despawnFigure(UseImage, UseAudio);
            gameObject.SetActive(false);
        }
    }
}
