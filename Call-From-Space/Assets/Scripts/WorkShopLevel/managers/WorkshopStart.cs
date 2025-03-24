using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopStart : MonoBehaviour
{
    public SaveManager saveManager;
    // Start is called before the first frame update

    public enum WorkshopSavePoint
    {
        Cockpit,
        FrontRoom,
        Gen1,
        Gen2
    }

    
    public WorkshopSavePoint workshopSavePoint;

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
        workshopSavePoint = WorkshopSavePoint.Cockpit;

        //special doors
        entryDoor.startsOpen = true;
        //endDoors
    }
    void SpawnInFrontRoom()
    {
        workshopSavePoint = WorkshopSavePoint.FrontRoom;
        FrontRoomDarkFigureTrigger.SetActive(false);

        //special doors
        entryDoor.startsOpen = false;
        frontRoomToMain.startsOpen = true;
        //endDoors

        //door Interactors
        authenticationPanel.active = false;
        //end door Interactors
    }
    void SpawnInGen1()
    {
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

    }
    void SpawnInGen2()
    {
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
