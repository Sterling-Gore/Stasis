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
        SaveData data = new SaveData {savepoint = savePoint};
        string json = JsonUtility.ToJson(data,true);
        heldData = data;
        File.WriteAllText(Application.persistentDataPath + "/savefile_" + slot + ".json", json);
        Debug.Log("Game saved at " + savePoint);
    }

    public SavePointID LoadSave(int slot = 1)
    {
        if(heldData == null)
        {
            DownloadSave(slot);
        }
        /*
        string savePath = Application.persistentDataPath + "/savefile_" + slot + ".json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data.savepoint;
        }
        UpdateSave(SavePointID.tutorial);
        return SavePointID.tutorial; // Default if no save exists
        */
        return heldData.savepoint;
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
            UpdateSave(SavePointID.tutorial);
            //heldData = new SaveData{savepoint = SavePointID.tutorial};
        }
        //UpdateSave(SavePointID.tutorial);
        //return SavePointID.tutorial; // Default if no save exists
    }

}

public class SaveData
{  
    public SavePointID savepoint;
}

public enum SavePointID
{
    tutorial,
    workshop1,
    workshop2,
    workshop3,
    workshop4
}
