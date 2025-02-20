using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineValveManager : MonoBehaviour
{
    // Start is called before the first frame update
    public engine_valve_interaction[] valves;
    bool PuzzleComplete = false;


    public Ship_door_and_button_controller engine_door;
    public Ship_button_interaction engine_button;
    public warp_drive_lockdown warp_drive;
    public OxygenSystem oxygenSystem; 

    void Start()
    {
        int count = 0;
        int numBroken = 0;
        float random_range = .5f;
        foreach(engine_valve_interaction valve in valves)
        {
            if(count >= valves.Length / 2 &&  numBroken <= 1)
            {
                random_range = .25f;
            }
            if( count == valves.Length-1 && numBroken == 0)
            {
                random_range = 0f;
            }
            if (Random.value > random_range)
            {
                valve.isBroken = true;
                valve.smoke.SetActive(true);
                numBroken += 1;
            }
            count += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateValve()
    {
        CheckValves();
    }

    void CheckValves()
    {
        int count = 0;
        foreach(engine_valve_interaction valve in valves)
        {
            if (!valve.smoke.activeSelf)
            {
                count += 1;
            }
        }
        if (count == valves.Length)
            CompletePuzzle();
    }

    void CompletePuzzle()
    {
        PuzzleComplete = true;
        engine_door.ToggleDoor(false, false);
        engine_button.off_until_special = true;
        warp_drive.Open = true;
        oxygenSystem.LosingOxygen = false;
        foreach(engine_valve_interaction valve in valves)
        {
            valve.isComplete = true;
        }
    }


}
