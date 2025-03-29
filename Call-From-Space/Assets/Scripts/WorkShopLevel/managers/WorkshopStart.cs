using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopStart : MonoBehaviour
{
    public SaveManager saveManager;
    public enum WorkshopSavePoint
    {
        Cockpit,
        FrontRoom,
        Gen1,
        Gen2
    }

    
    public WorkshopSavePoint workshopSavePoint;
    // Start is called before the first frame update

    public Transform camera;
    public Rigidbody player_rb;
    [Header("Player Position")]
    public Vector3 cockpitPosition;
    public Vector3 frontRoomPosition;
    public Vector3 genAPosition;
    public Vector3 genBPosition;

    [Header("Player Rotation")]
    public Vector3 cockpitRotation;
    public Vector3 frontRoomRotation;
    public Vector3 genARotation;
    public Vector3 genBRotation;


    [Header("Items")]
    public PickUp OrangeKeyCard; 
    public PickUp PurpleKeyCard; 
    public PickUp BlueKeyCard; 
    public PickUp StickyNote; 

    [Header("Dark Figure Events")]
    public GameObject FrontRoomDarkFigureTrigger;
    public GameObject VentDarkFigureTrigger;
    public GameObject ConferenceRoomDarkFigureTrigger;

    [Header("Special Doors")]
    public PowerDoors_Workshop entryDoor;
    public PowerDoors_Workshop frontRoomToMain;
    public PowerDoors_Workshop genADoor;
    public PowerDoors_Workshop genBDoor;
    public PowerDoors_Workshop RampDoor;
    public PowerDoors_Workshop BedRoomDoor;

    [Header("Door Interactors")]
    public AuthenticationPanel authenticationPanel;
    public SwiperForKeyCard SwiperGenA;
    public SwiperForKeyCard SwiperRamp;
    public SwiperForKeyCard SwiperGenB;
    public BustedDoorTrigger bustedDoorTrigger;

    [Header("Map Components")]
    public MapManager mapManager;
    public GameObject tutorialMap;
    public GameObject workshopMap;

    [Header("Oxygen")]
    public OxygenSystem oxygenSystem;



    void Awake()
    {
        SavePointID savePoint = saveManager.LoadSave();
        switch (savePoint)
        {
            case SavePointID.workshop1:
                SpawnInCockPit();
                break;
            case SavePointID.workshop2:
                SpawnInFrontRoom();
                break;
            case SavePointID.workshop3:
                SpawnInGen1();
                break;
            case SavePointID.workshop4:
                SpawnInGen2();
                break;
            default:
                saveManager.UpdateSave(SavePointID.workshop1);
                SpawnInCockPit();
                break;
        }
    }

    void Start() //runs after awake
    {
        onStart(workshopSavePoint);
    }

    void SpawnInCockPit()
    {
        player_rb.position = cockpitPosition;
        camera.rotation = Quaternion.Euler(cockpitRotation.x, cockpitRotation.y, cockpitRotation.z);

        workshopSavePoint = WorkshopSavePoint.Cockpit;

        //special doors
        entryDoor.startsOpen = true;
        //endDoors
        mapManager.SetMap(tutorialMap);
        oxygenSystem.LosingOxygen = false;
    }
    void SpawnInFrontRoom()
    {
        player_rb.position = frontRoomPosition;
        camera.rotation = Quaternion.Euler(frontRoomRotation.x, frontRoomRotation.y, frontRoomRotation.z);

        workshopSavePoint = WorkshopSavePoint.FrontRoom;
        FrontRoomDarkFigureTrigger.SetActive(false);

        //special doors
        entryDoor.startsOpen = false;
        frontRoomToMain.startsOpen = true;
        //endDoors

        //door Interactors
        authenticationPanel.active = false;
        //end door Interactors
        mapManager.SetMap(workshopMap);
        oxygenSystem.LosingOxygen = true;
    }
    void SpawnInGen1()
    {
        player_rb.position = genAPosition;
        camera.rotation = Quaternion.Euler(genARotation.x, genARotation.y, genARotation.z);

        workshopSavePoint = WorkshopSavePoint.Gen1;
        FrontRoomDarkFigureTrigger.SetActive(false);
        VentDarkFigureTrigger.SetActive(false);
        

        //special doors
        entryDoor.startsOpen = false;
        frontRoomToMain.startsOpen = true;
        genADoor.startsOpen = true;
        BedRoomDoor.startsBroken = true;
        //endDoors
        
        //door Interactors
        authenticationPanel.active = false;
        SwiperGenA.active = false;
        bustedDoorTrigger.active = false;
        //end door Interactors

        mapManager.SetMap(workshopMap);
        oxygenSystem.LosingOxygen = true;

    }
    void SpawnInGen2()
    {
        player_rb.position = genBPosition;
        camera.rotation = Quaternion.Euler(genBRotation.x, genBRotation.y, genBRotation.z);

        workshopSavePoint = WorkshopSavePoint.Gen2;
        FrontRoomDarkFigureTrigger.SetActive(false);
        VentDarkFigureTrigger.SetActive(false);
        ConferenceRoomDarkFigureTrigger.SetActive(false);

        //special doors
        entryDoor.startsOpen = false;
        frontRoomToMain.startsOpen = true;
        genADoor.startsOpen = true;
        BedRoomDoor.startsBroken = true;
        RampDoor.startsOpen = true;
        genBDoor.startsOpen = true;
        //endDoors

        //door Interactors
        authenticationPanel.active = false;
        SwiperGenA.active = false;
        bustedDoorTrigger.active = false;
        SwiperGenB.active = false;
        SwiperRamp.active = false;
        //end door Interactors

        mapManager.SetMap(workshopMap);
        oxygenSystem.LosingOxygen = false;


        
        
    }

    void onStart(WorkshopSavePoint savePoint)
    {
        if(savePoint == WorkshopSavePoint.Gen1)
        {
            StickyNote.pickUp();
            OrangeKeyCard.deletedPickUp();
        }
        else if(savePoint == WorkshopSavePoint.Gen2)
        {
            StickyNote.pickUp();
            OrangeKeyCard.deletedPickUp();
            PurpleKeyCard.deletedPickUp();
            BlueKeyCard.deletedPickUp();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
