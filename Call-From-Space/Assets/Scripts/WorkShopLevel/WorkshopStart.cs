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

    void SpawnInCockPit()
    {
        workshopSavePoint = WorkshopSavePoint.Cockpit;
    }
    void SpawnInFrontRoom()
    {
        workshopSavePoint = WorkshopSavePoint.FrontRoom;
    }
    void SpawnInGen1()
    {
        workshopSavePoint = WorkshopSavePoint.Gen1;
    }
    void SpawnInGen2()
    {
        workshopSavePoint = WorkshopSavePoint.Gen2;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
