using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPuzzleManager : MonoBehaviour
{
    private bool enterBloodVial;
    private bool enterPlantVial;
    private bool enterBattery;
    private float timerForCompletion;
    public SaveManager saveManager;

    [Header("Laser Scripts")]
    public LaserScript wall1Origin;
    public LaserScript wall2Origin;
    public LaserScript[] wallReflectors;
    public LaserScript endpoint1;
    public LaserScript endpoint2;

    [Header("Reflector Colliders")]
    public Collider[] wallReflectorColliders;


    [Header("Insert Objects")]
    public GameObject bloodVial;
    public GameObject plantVial;
    public GameObject battery;
    public GameObject emptyVials;


    [Header("Antidote")]
    public GameObject viewAntidote;
    public GameObject holdAntidote;

    [Header("Manager")]
    public bool puzzleIsCompleted = false;
    public bool spawnAntidote = false;
    public GameObject deposits;

    [Header("Lights")]
    public Color lightOnColor;
    public Material LightOn; 
    public Color lightOffColor;
    public Material LightOff;
    public Renderer Light1Bulb;
    public Light Light1;
    public Renderer Light2Bulb;
    public Light Light2;
    public Renderer Light3Bulb;
    public Light Light3;

    [Header("Audios")]
    public AudioSource dingAudio;
    public AudioSource mixingAudio;
    public AudioSource completionAudio;

    bool wall1EndOn = false;
    bool wall2EndOn = false;


    // Start is called before the first frame update
    void Start()
    {
        timerForCompletion = 0;
        if(puzzleIsCompleted)
        {
            insertAll();
            allRed();
            despawnAllColliders();
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
            if(!wall1EndOn && endpoint1.GetComponent<LaserScript>().on)
            {
                wall1EndOn = true;
                Light2Bulb.material = LightOn;
                Light2.color = lightOnColor;
            }
            else if(wall1EndOn && !endpoint1.GetComponent<LaserScript>().on)
            {
                wall1EndOn = false;
                Light2Bulb.material = LightOff;
                Light2.color = lightOffColor;
            }
            if(!wall2EndOn && endpoint2.GetComponent<LaserScript>().on)
            {
                wall2EndOn = true;
                Light3Bulb.material = LightOn;
                Light3.color = lightOnColor;
            }
            else if(wall2EndOn && !endpoint2.GetComponent<LaserScript>().on)
            {
                wall2EndOn = false;
                Light3Bulb.material = LightOff;
                Light3.color = lightOffColor;
            }
            if(CheckCompletion())
            {
                puzzleIsCompleted = true;
                StartCoroutine(StartMixing());
            }
        }
    }

    IEnumerator StartMixing()
    {
        dingAudio.Play();
        yield return new WaitForSeconds(.5f);
        mixingAudio.Play();
        yield return new WaitForSeconds(6f);
        completionAudio.Play();
        despawnAllLasers();
        emptyVials.SetActive(true);
        bloodVial.SetActive(false);
        plantVial.SetActive(false);
        despawnAllColliders();
        createAntidote();
        saveManager.UpdateSave(SavePointID.research3);
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
        emptyVials.SetActive(true);
        //bloodVial.SetActive(true);
        //plantVial.SetActive(true);
        battery.SetActive(true);
        deposits.SetActive(false);
    }

    public void updateInserts()
    {
        if(enterBloodVial && enterPlantVial)
        {
            wall1Origin.on = true;
            wall1Origin.laserHum.enabled = true;
            Light1Bulb.material = LightOn;
            Light1.color = lightOnColor;
            dingAudio.Play();
            if(enterBattery)
            {
                wall2Origin.on = true;
                wall2Origin.laserHum.enabled = true;
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

    void despawnAllColliders()
    {
        foreach (Collider collider in wallReflectorColliders)
        {
            collider.enabled = false;
        }
    }

    void createAntidote()
    {
        viewAntidote.SetActive(false);
        holdAntidote.SetActive(true);
    }

    void allRed()
    {
        Light1Bulb.material = LightOff;
        Light1.color = lightOffColor;
        Light2Bulb.material = LightOff;
        Light2.color = lightOffColor;
        Light3Bulb.material = LightOff;
        Light3.color = lightOffColor;
    }
}
