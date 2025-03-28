using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SaveManager saveManager;
    // Start is called before the first frame update
    public void LoadSceneFromSavePoint()
    {
        SavePointID savePoint = saveManager.LoadSave();
        string sceneName ="";
        switch (savePoint)
        {
            case SavePointID.intro:
                sceneName = "intro";
                break;
            case SavePointID.tutorial:
                sceneName = "Tutorial";
                break;
            case SavePointID.workshop1:
                sceneName = "Workshop";
                break;
            case SavePointID.workshop2:
                sceneName = "Workshop";
                break;
            case SavePointID.workshop3:
                sceneName = "Workshop";
                break;
            case SavePointID.workshop4:
                sceneName = "Workshop";
                break;
            default:
                break;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
        else
        {
            SceneManager.LoadSceneAsync("Start");
        }
    }
}
