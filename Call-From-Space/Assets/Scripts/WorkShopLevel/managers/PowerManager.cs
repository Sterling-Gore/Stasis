using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
 
    
    public Color lightOnColor;
    public Material LightOn; // for when the light turns on

    public OxygenSystem oxygenSystem;

    [System.Serializable]
    public class PowerZone
    {
        public string zoneName;
        public GameObject[] lights;
        public PowerDoors_Workshop[] doors;
        public GameObject[] lightCones;
        public Renderer ElevatorLightBulb;
        public Light ElevatorLight;
        
        //public GameObject ElevatorLight;
    }

    [Header("Power Zones")]
    public PowerZone zoneA;
    public PowerZone zoneB;

    [Header("Elevator")]
    public ElevatorDoor elevatorDoor;




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
        elevatorDoor.active = true;
        oxygenSystem.LosingOxygen = false;
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
        zone.ElevatorLight.color = lightOnColor;
        zone.ElevatorLightBulb.material = LightOn;
    }


}
