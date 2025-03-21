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
        switch (workshopSavePoint)
        {
            case WorkshopSavePoint.Cockpit:
                SpawnInCockPit(true);
                break;
            case WorkshopSavePoint.FrontRoom:
                SpawnInFrontRoom(true);
                break;
            case WorkshopSavePoint.Gen1:
                SpawnInGen1(true);
                break;
            case WorkshopSavePoint.Gen2:
                SpawnInGen2(true);
                break;
            default:
                SpawnInCockPit(true);
                break;
        }
    }

    void SpawnInCockPit(bool onStart = false)
    {
        workshopSavePoint = WorkshopSavePoint.Cockpit;
    }
    void SpawnInFrontRoom(bool onStart = false)
    {
        workshopSavePoint = WorkshopSavePoint.FrontRoom;
    }
    void SpawnInGen1(bool onStart = false)
    {
        workshopSavePoint = WorkshopSavePoint.Gen1;
        if(onStart)
        {
            StickyNote.pickUp();
            OrangeKeyCard.deletedPickUp();
            
        }
    }
    void SpawnInGen2(bool onStart = false)
    {
        workshopSavePoint = WorkshopSavePoint.Gen2;
        if(onStart)
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
