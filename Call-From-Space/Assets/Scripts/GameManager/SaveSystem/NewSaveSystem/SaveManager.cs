using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{

    SaveData heldData = null;
    string GetSavePath(int slot = 1)
    {
        return Application.persistentDataPath + "/savefile_" + slot + ".json";
    }

    public void UpdateSave(SavePointID savePoint, int slot = 1)
    {
        float Volume = LoadVolume();
        float Sensitivity = LoadSensitivity();
        SaveData data = new SaveData {savepoint = savePoint, volume = Volume, sensitivity = Sensitivity};
        string json = JsonUtility.ToJson(data,true);
        heldData = data;
        File.WriteAllText(Application.persistentDataPath + "/savefile_" + slot + ".json", json);
        Debug.Log("Game saved at " + savePoint);
    }

    public void UpdateSettings(float Volume, float Sensitivity, int slot = 1)
    {
        SavePointID savePoint =  LoadSave();
        SaveData data = new SaveData {savepoint = savePoint, volume = Volume, sensitivity = Sensitivity};
        string json = JsonUtility.ToJson(data,true);
        heldData = data;
        Debug.Log(heldData);
        File.WriteAllText(Application.persistentDataPath + "/savefile_" + slot + ".json", json);
    }

    public void UpdateAll(SavePointID savePoint, float Volume, float Sensitivity, int slot = 1)
    {
        SaveData data = new SaveData {savepoint = savePoint, volume = Volume, sensitivity = Sensitivity};
        string json = JsonUtility.ToJson(data,true);
        heldData = data;
        File.WriteAllText(Application.persistentDataPath + "/savefile_" + slot + ".json", json);
    }

    public SavePointID LoadSave(int slot = 1)
    {
        if(heldData == null || heldData.savepoint == null)
        {
            DownloadSave(slot);
        }
        return heldData.savepoint;
    }

    public float LoadVolume(int slot = 1)
    {
        if(heldData == null)
        {
            DownloadSave(slot);
        }
        if(heldData.volume == null)
        {
            heldData.volume = -10;
        }
        return heldData.volume;
    }

    public float LoadSensitivity(int slot = 1)
    {
        if(heldData == null )
        {
            DownloadSave(slot);
        }
        if(heldData.sensitivity == null)
        {
            heldData.sensitivity = 6;
        }
        return heldData.sensitivity;
    }

    private void DownloadSave(int slot = 1)
    {
        string savePath = Application.persistentDataPath + "/savefile_" + slot + ".json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            heldData =  data;
        }
        else
        {
            UpdateAll(SavePointID.tutorial, -10, 6);
            //heldData = new SaveData{savepoint = SavePointID.tutorial};
        }
        //UpdateSave(SavePointID.tutorial);
        //return SavePointID.tutorial; // Default if no save exists
    }

}

public class SaveData
{  
    public SavePointID savepoint;
    public float volume;
    public float sensitivity;
}

public enum SavePointID
{
    intro,
    tutorial,
    workshop1,
    workshop2,
    workshop3,
    workshop4,
    research1,
    research2,
    research3,
    research4,
    research5
}
