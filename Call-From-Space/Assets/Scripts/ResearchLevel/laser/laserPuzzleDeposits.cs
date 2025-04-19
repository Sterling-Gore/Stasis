using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserPuzzleDeposits : Interactable
{
    // Start is called before the first frame update
    public GameObject PickUpObject;
    public GameObject Player;
    public GameObject sparkle;
    public AudioSource placeDownAudio;
    public laserPuzzleManager laserManager;

    public bool isBattery;
    public bool isPlantVial;
    public bool isBloodVial;

    public string name;


    public override string GetDescription()
    {
        if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(PickUpObject.GetComponent<Item>()))
            return "<color=red>Press [E]</color=red> to Insert " + name;
        return "Find " + name;
    }

    public override void Interact()
    {
        if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(PickUpObject.GetComponent<Item>()))
        {
            Player.GetComponent<UI_Controller>().inventory.DeleteItem(PickUpObject.GetComponent<Item>());
            if(isBattery)
            {
                laserManager.insertBattery();
            }
            else if(isPlantVial)
            {
                laserManager.insertPlantVial();
            }
            else if(isBloodVial)
            {
                laserManager.insertBloodVial();
            }
            if(sparkle != null)
                sparkle.SetActive(false);
            placeDownAudio.Play();
            gameObject.SetActive(false);
            
        }
    }
}
