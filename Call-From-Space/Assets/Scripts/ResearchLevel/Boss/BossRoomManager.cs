using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public OverideElectircPanel[] panels;
    public GameObject[] sparkles;
    public GameObject[] plants;
    public PlasmaGun gun;
    public GameObject DoorCollider;
    public GameObject entranceCollider;
    public GameObject[] PanelLightings;

    void Awake()
    {
        //puzzleIsDone();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void puzzleIsDone()
    {
        foreach (GameObject panelLightning in PanelLightings)
        {
            panelLightning.SetActive(false);
        }
        foreach (OverideElectircPanel panel in panels)
        {
            panel.PuzzleCompleted = true;
        }
        foreach (GameObject sparkle in sparkles)
        {
            sparkle.SetActive(false);
        }
        foreach (GameObject plant in plants)
        {
            plant.SetActive(false);
        }
        DoorCollider.SetActive(false);
        entranceCollider.SetActive(false);
        gun.done = true;
    }
}
