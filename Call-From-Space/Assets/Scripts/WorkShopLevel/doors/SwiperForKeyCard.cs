using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiperForKeyCard : Interactable
{
    public PowerDoors_Workshop door;
    public bool active = true;
    public GameObject sparkle;
    public string colorCard;

    public GameObject Player;
    public GameObject Key;
    // Start is called before the first frame update
    void Start()
    {
        if(!active)
        {
            sparkle.SetActive(false);
        }
    }


    public override string GetDescription()
    { 
        if (active)
        {
            if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(Key.GetComponent<Item>()))
            {
                return "<color=red>Press [E]</color=red> to Unlock Door";
            }
            else
            {
                return "Needs " + colorCard + " Access Keycard";
            }
        }
        else
        {
            return "";
        }

    }

    public override void Interact()
    {
        if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(Key.GetComponent<Item>()))
        {
            sparkle.SetActive(false);
            active = false;
            Player.GetComponent<UI_Controller>().inventory.DeleteItem(Key.GetComponent<Item>());
            door.PowerOn();
        }

    }
}
