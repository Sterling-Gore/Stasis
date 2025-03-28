using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public CameraController cameraController;
    public float Volume = -10;
    public float Sensitivity = 6;
    public settingsManager settings_manager;

    [Header("Sliders")]
    public Slider sliderVolume;
    public Slider sliderSensitivity;

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        Debug.Log(volume);
        Volume = volume;
    }
    public void setSensitivity(float sens)
    {
        if (sens == 1)
            cameraController.mouseSensitivity = 25;
        else
            cameraController.mouseSensitivity = 50 * (sens - 1);
        Sensitivity = sens;
    }

    void OnDisable()
    {
        settings_manager.SaveSettings(Volume, Sensitivity);
    }

    void OnEnable()
    {
        float tempVolume;
        if (audioMixer.GetFloat("volume", out tempVolume)) 
        {
            sliderVolume.value = tempVolume;
            Volume = tempVolume;
        }
        else
        {
            sliderVolume.value = -10;
            Volume = -10;
        }


        if(cameraController.mouseSensitivity == 25)
        {
            sliderSensitivity.value = 1;
            Sensitivity = 1;
        }
        else
        {
            sliderSensitivity.value = (cameraController.mouseSensitivity / 50) + 1;
            Sensitivity = (cameraController.mouseSensitivity / 50) + 1;
        }

    }

    
}
