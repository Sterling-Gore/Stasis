using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPuzzleManager : MonoBehaviour
{
    private bool enterBloodVial;
    private bool enterPlantVial;
    private bool enterBattery;
    private float timerForCompletion;

    [Header("Laser Scripts")]
    public LaserScript wall1Origin;
    public LaserScript wall2Origin;
    public LaserScript[] wallReflectors;
    public LaserScript endpoint1;
    public LaserScript endpoint2;


    [Header("Insert Objects")]
    public GameObject bloodVial;
    public GameObject plantVial;
    public GameObject battery;


    [Header("Antidote")]
    public Collider antidoteCollider;
    public Renderer antidoteMeshRenderer;
    public Rigidbody antidoteRigidBody;

    [Header("Manager")]
    public bool puzzleIsCompleted = false;
    public bool spawnAntidote = false;

    // Start is called before the first frame update
    void Start()
    {
        timerForCompletion = 0;
        if(puzzleIsCompleted)
        {
            insertAll();
        }
        if(spawnAntidote)
        {
            createAntidote();   
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!puzzleIsCompleted)
        {
            if(CheckCompletion())
            {
                puzzleIsCompleted = true;
                despawnAllLasers();
                createAntidote();
            }
        }
    }

    public void insertBloodVial()
    {
        bloodVial.SetActive(true);
        enterBloodVial = true;
        updateInserts();
    }

    public void insertPlantVial()
    {
        plantVial.SetActive(true);
        enterPlantVial = true;
        updateInserts();
    }

    public void insertBattery()
    {
        battery.SetActive(true);
        enterBattery = true;
        updateInserts();
    }

    public void insertAll()
    {
        bloodVial.SetActive(true);
        plantVial.SetActive(true);
        battery.SetActive(true);
    }

    public void updateInserts()
    {
        if(enterBloodVial && enterPlantVial)
        {
            wall1Origin.on = true;
            if(enterBattery)
            {
                wall2Origin.on = true;
            }
        }
    }



    bool CheckCompletion()
    {
        if(endpoint1.GetComponent<LaserScript>().on && endpoint2.GetComponent<LaserScript>().on)
        {
            if(timerForCompletion > 2f)
            {
                return true;
            }
            timerForCompletion += Time.deltaTime;
        }
        else
        {
            timerForCompletion = 0;
        }
        return false;
    }

    void despawnAllLasers()
    {
        wall1Origin.on = false;
        wall1Origin.laserHum.enabled = false;
        wall2Origin.on = false;
        wall2Origin.laserHum.enabled = false;
        foreach( LaserScript laser in wallReflectors)
        {
            laser.on = false;
            laser.laserHum.enabled = false;
        }
    }

    void createAntidote()
    {
        antidoteCollider.enabled = true;
        antidoteRigidBody.isKinematic = false;
    }
}
