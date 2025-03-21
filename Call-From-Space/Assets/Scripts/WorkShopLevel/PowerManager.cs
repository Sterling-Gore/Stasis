using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
 

    
    [System.Serializable]
    public class PowerZone
    {
        public string zoneName;
        public GameObject[] lights;
        public PowerDoors_Workshop[] doors;
        public GameObject[] lightCones;
    }

    [Header("Power Zones")]
    public PowerZone zoneA;
    public PowerZone zoneB;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void turnOnGenA()
    {
        turnPowerOn(zoneA);
    }
    public void turnOnGenB()
    {
        turnPowerOn(zoneB);
    }

    void turnPowerOn(PowerZone zone)
    {
        foreach (GameObject light in zone.lights)
        {
            light.SetActive(true);
        }
        foreach (GameObject lightCone in zone.lightCones)
        {
            lightCone.SetActive(true);
        }
        foreach (PowerDoors_Workshop door in zone.doors)
        {
            door.PowerOn();
        }
    }


}
