using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : Loadable
{
    public GameObject LightBar;
    public Light light;
    public bool playSound;
    public AudioSource audioSource;
    public AudioClip flashlightSoundOn;
    public AudioClip flashlightSoundOff;
    public AudioClip breakingLightSound;
    public UI_Controller ui_controller;

    public float flashlightTimer;
    public bool isOn;


    void Start()
    {
        light = GetComponent<Light>();
        light.enabled = false;
        flashlightTimer = 45f;
        isOn = false;
    }


    void Update()
    {
        if(ui_controller.UI_Value == UI_Controller.UI_Types.viewing_screen && isOn)
        {
            toggleLight();
        }
        if (Input.GetKeyUp(KeyCode.F) && ui_controller.IsSpaceSuitOn && ui_controller.UI_Value != UI_Controller.UI_Types.viewing_screen)
        {
            toggleLight();
        }

        if (isOn)
            flashlightTimer = Mathf.Clamp(flashlightTimer - Time.deltaTime, 0, 45);
        else
            flashlightTimer = Mathf.Clamp(flashlightTimer + (3 * Time.deltaTime), 0, 45);

        if (flashlightTimer == 0)
            toggleLight();

        if (playSound) // playsound is only on one flashlight, that way this only gets called on one light
        {
            if (flashlightTimer < 45)
            {
                LightBar.SetActive(true);
                LightBar.transform.Find("Flashlight_filled").gameObject.GetComponent<Image>().fillAmount = flashlightTimer / 45;
            }
            else //light is filled
            {
                LightBar.SetActive(false);
            }

        }

    }

    void toggleLight()
    {
        if (playSound)
        {
            audioSource.clip = light.enabled ? flashlightSoundOff : flashlightSoundOn;
            audioSource.PlayOneShot(clip: audioSource.clip);
        }
        if (isOn)
        {
            light.enabled = false;
        }
        else
        {
            light.enabled = true;
        }
        isOn = !isOn;
    }

    public void turnOffLight()
    {
        if (isOn)
        {
            light.enabled = false;
            if (playSound)
            {
                audioSource.clip = breakingLightSound;
                audioSource.PlayOneShot(clip: audioSource.clip);
            }
            isOn = !isOn;
        }
    }

    public override void Load(JObject state)
    {
        flashlightTimer = 45;
    }

    public override void Save(ref JObject state)
    {
        state["flashLightTime"] = flashlightTimer;
    }
}