using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor :  Interactable
{
    
    public bool active = false;
    public bool complete = false;
    public GameObject sparkle;
    public string colorCard;

    public GameObject Player;
    public GameObject Key;

    [Header("Animation")]
    public Animator elevatorAnimation;

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
        if(complete)
        {
            return "";
        }
        else if (active)
        {
            if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(Key.GetComponent<Item>()))
            {
                return "<color=red>Press [E]</color=red> to Unlock Elevator Door";
            }
            else
            {
                return "Needs " + colorCard + " Access Keycard";
            }
        }
        else
        {
            return "Elevator Needs Power";
        }

    }

    public override void Interact()
    {
        if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(Key.GetComponent<Item>()) && active)
        {
            sparkle.SetActive(false);
            complete = true;
            active = false;
            Player.GetComponent<UI_Controller>().inventory.DeleteItem(Key.GetComponent<Item>());
            elevatorAnimation.SetTrigger("open");
        }

    }
}
