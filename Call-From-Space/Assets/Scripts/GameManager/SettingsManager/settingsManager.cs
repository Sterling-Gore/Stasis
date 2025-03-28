using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class settingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public CameraController cameraController;
    // Start is called before the first frame update
    public SaveManager saveManager;
    void Start()
    {
        audioMixer.SetFloat("volume", saveManager.LoadVolume());
        float sensitvity = saveManager.LoadSensitivity();
        if (sensitvity == 1)
            cameraController.mouseSensitivity = 25;
        else
            cameraController.mouseSensitivity = 50 * (sensitvity - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSettings(float Volume, float Sensitivity)
    {
        saveManager.UpdateSettings(Volume, Sensitivity);
        Debug.Log("SAVING SETTINGS");
    }
}
