using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class DrawerInteraction : Interactable
{
    // Start is called before the first frame update
    public Animator animation;
    public GameObject Player;
    public GameObject Key;
    bool unlocked = false;

    public AudioSource audioSource;

    public GameObject Sparkle;
    void Start()
    {
        if(animation == null)
            animation = GetComponent<Animator>();
    }

    public override string GetDescription()
    {
        if (!unlocked)
        {
            if (Player.GetComponent<UI_Controller>().inventory.IsItemInList(Key.GetComponent<Item>()))
            {
                return "Press [E] to Unlock Drawer";
            }
            else
            {
                return "Needs a key";
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
            openDrawer();
            //Player.GetComponent<UI_Controller>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle2(3);
        }

    }

    public void openDrawer()
    {
        Sparkle.SetActive(false);
        unlocked = true;
        animation.SetBool("Opened", true);
        audioSource.enabled = true;
        Player.GetComponent<UI_Controller>().inventory.DeleteItem(Key.GetComponent<Item>());
    }

    /*public override void Load(JObject state)
    {
        base.Load(state);
        unlocked = (bool)state[fullName]["unlocked"];
        Sparkle.SetActive(!unlocked);
        animation.SetBool("Opened", unlocked);
    }

    public override void Save(ref JObject state)
    {
        base.Save(ref state);
        state[fullName]["unlocked"] = unlocked;
    }
    */
}
