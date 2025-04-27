using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class researchLevelStart : MonoBehaviour
{
    public SaveManager saveManager;
    public SceneStart sceneStart;

    public enum ResearchSavePoint
    {
        elevator,
        lab,
        laser,
        simonSays,
        boss
    }

    public ResearchSavePoint researchSavePoint;

    public Transform camera;
    public Rigidbody player_rb;


    [Header("Player Position")]
    public Vector3 elevatorPosition;
    public Vector3 labPosition;
    public Vector3 LaserPosition;
    public Vector3 simonSaysPosition;
    public Vector3 bossPosition;

    [Header("Player Rotation")]
    public Vector3 elevatorRotation;
    public Vector3 labRotation;
    public Vector3 LaserRotation;
    public Vector3 simonSaysRotation;
    public Vector3 bossRotation;


    [Header("Items")]
    public PickUp bloodVial; 
    public PickUp plantVial; 
    public PickUp drawerKey; 
    public PickUp reflector; 

    [Header("Holdables")]
    public GameObject valve; 
    public GameObject plant;
    public GameObject antidote;

    [Header("Elevator Deliverables")]
    public Animator elevatorDoorAnimation;
    public AudioSource elevatorMusic;

    [Header("Lab Puzzle Deliverables")]
    public labManager lab_manager;
    public GameObject bloodLabVial;
    public GameObject plantLabVial;

    [Header("Laser Puzzle Deliverables")]
    public laserPuzzleManager laser_manager;
    public DrawerInteraction drawer;

    [Header("Extra Audios")]
    public AudioSource elevatorEntrance;


    void Awake()
    {
        SavePointID savePoint = saveManager.LoadSave();
        switch(savePoint)
        {
            case SavePointID.research1:
                SpawnInElevator();
                break;
            case SavePointID.research2:
                SpawnInLab();
                break;
            case SavePointID.research3:
                SpawnInLaser();
                break;
            case SavePointID.research4:
                SpawnInSimonSays();
                break;
            case SavePointID.research5:
                SpawnInBoss();
                break;
            default:
                saveManager.UpdateSave(SavePointID.research1);
                SpawnInElevator();
                break;
        }

    }
    
    void Start()
    {
        onStart(researchSavePoint);
    }

    void onStart(ResearchSavePoint savePoint)
    {
        if(savePoint == ResearchSavePoint.elevator)
        {
            StartCoroutine(elevatorSequence());
        }
        else if(savePoint == ResearchSavePoint.lab)
        {
            bloodVial.pickUp();
            plantVial.pickUp();
            valve.SetActive(false);
            plant.SetActive(false);
        }
        else if(savePoint == ResearchSavePoint.laser)
        {
            bloodVial.deletedPickUp();
            plantVial.deletedPickUp();
            reflector.deletedPickUp();
            drawerKey.deletedPickUp();
            valve.SetActive(false);
            plant.SetActive(false);
        }
        else if(savePoint == ResearchSavePoint.simonSays)
        {
            bloodVial.deletedPickUp();
            plantVial.deletedPickUp();
            reflector.deletedPickUp();
            drawerKey.deletedPickUp();
            valve.SetActive(false);
            plant.SetActive(false);
            antidote.SetActive(false);
        }
        else if(savePoint == ResearchSavePoint.boss)
        {
            bloodVial.deletedPickUp();
            plantVial.deletedPickUp();
            reflector.deletedPickUp();
            drawerKey.deletedPickUp();
            valve.SetActive(false);
            plant.SetActive(false);
            antidote.SetActive(false);
        }
    }

    void SpawnInElevator()
    {
        researchSavePoint = ResearchSavePoint.elevator;

        sceneStart.delayAudio = false;
        player_rb.position = elevatorPosition;
        camera.rotation = Quaternion.Euler(elevatorRotation.x, elevatorRotation.y, elevatorRotation.z);
    }

    void SpawnInLab()
    {
        elevatorDoorAnimation.SetTrigger("open");
        researchSavePoint = ResearchSavePoint.lab;

        lab_manager.complete_for_awake();
        bloodLabVial.SetActive(false);
        plantLabVial.SetActive(false);

        sceneStart.delayAudio = true;
        player_rb.position = labPosition;
        camera.rotation = Quaternion.Euler(labRotation.x, labRotation.y, labRotation.z);
    }

    void SpawnInLaser()
    {
        elevatorDoorAnimation.SetTrigger("open");
        researchSavePoint = ResearchSavePoint.laser;

        lab_manager.complete_for_awake();
        bloodLabVial.SetActive(false);
        plantLabVial.SetActive(false);
        laser_manager.puzzleIsCompleted = true;
        laser_manager.spawnAntidote = true;
        drawer.openDrawer();

        sceneStart.delayAudio = true;
        player_rb.position = LaserPosition;
        camera.rotation = Quaternion.Euler(LaserRotation.x, LaserRotation.y, LaserRotation.z);
    }

    void SpawnInSimonSays()
    {
        elevatorDoorAnimation.SetTrigger("open");
        researchSavePoint = ResearchSavePoint.simonSays;

        sceneStart.delayAudio = true;
        bloodLabVial.SetActive(false);
        plantLabVial.SetActive(false);
        player_rb.position = simonSaysPosition;
        camera.rotation = Quaternion.Euler(simonSaysRotation.x, simonSaysRotation.y, simonSaysRotation.z);
    }

    void SpawnInBoss()
    {
        researchSavePoint = ResearchSavePoint.boss;

        sceneStart.delayAudio = true;
        player_rb.position = bossPosition;
        camera.rotation = Quaternion.Euler(bossPosition.x, bossPosition.y, bossPosition.z);
    }




    IEnumerator elevatorSequence()
    {
        yield return new WaitForSeconds(.1f);
        elevatorMusic.Play();
        yield return new WaitForSeconds(9f);
        elevatorDoorAnimation.SetTrigger("open");

    }
}
