using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PickUp : Interactable
{
    Item item;
    public GameObject player;
    UI_Controller ui_controller;
    public GameObject JournalPlayer;
    public GameObject ItemGlow;
    public AudioClip PickUpSound;

    public AudioSource audioSource;


    [Header("TUTORIAL")]
    public HelpTexts helptextForRadio;
    public Ship_button_interaction ButtonForRadio;
    public GameObject TriggerForCockpit;



    override protected void Awake()
    {
        base.Awake();
        item = GetComponent<Item>();
        ui_controller = player.GetComponent<UI_Controller>();

        //Physics.IgnoreCollision(transform.Find("Collider").GetComponent<Collider>(), player.transform.Find("Player Model").GetComponent<Collider>(), true);
    }

    void Update()
    {
        if (gameObject.activeSelf && ItemGlow.activeSelf)
            ItemGlow.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
    }

    public override string GetDescription()
    {

        return ("Press [E] to pick up " + item.itemName);
    }

    public override void Interact()
    {
        //inventory.AddItem(item);
        audioSource.PlayOneShot(PickUpSound);
        ItemGlow.SetActive(false);
        switch (item.itemName)
        {
            case "Sticky Note":
                player.GetComponent<PlayerController>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle1(2);
                break;
            case "Locker Key":
                player.GetComponent<PlayerController>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle2(2);
                break;
            case "Radio Transmitter":
                helptextForRadio.PressTAB = true;
                ButtonForRadio.off_until_special = false;
                TriggerForCockpit.SetActive(true);
                break;
            default:
                break;
        }

        if (!item.isItem)
        {
            JournalPlayer.GetComponent<PlayJournal>().PlayAudioOnPickUp(item);
        }

        gameObject.SetActive(false);
        if (item.isItem)
        {
            ui_controller.inventory.AddItem(item);
        }
        else
        {
            ui_controller.inventory.AddJournal(item);
        }

        //ui_controller.uiInvetory.RefreshInventoryItems();

    }

    public override void Load(JObject state)
    {
        var active = (bool)state[fullName]["isActive"];
        gameObject.SetActive(active);
        ItemGlow.SetActive(active);
        base.Load(state);
    }

    public override void Save(ref JObject state)
    {
        base.Save(ref state);
        state[fullName]["isActive"] = gameObject.activeSelf;
    }
}
