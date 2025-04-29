using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class PickUp : Interactable
{
    public Item item;
    public GameObject player;
    public UI_Controller ui_controller;
    public GameObject JournalPlayer;
    public GameObject ItemGlow;
    public AudioClip PickUpSound;

    public AudioSource audioSource;


    [Header("TUTORIAL")]
    public HelpTexts helptextForRadio;
    public Ship_button_interaction ButtonForRadio;
    public GameObject TriggerForCockpit;

    [Header("WOEKSHOP")]
    public ManagerDarkFigure DarkFigureForPurpleKey;
    public AlienController alienController;

    public event EventHandler<PickUpEventArgs> ItemPickedUp;

    override protected void Awake()
    {
        base.Awake();
        if(item == null)
            item = GetComponent<Item>();
        if(ui_controller == null)
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
        switch (item.itemName)
        {
            case "Green Access Key":
                //player.GetComponent<PlayerController>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle2(2);
                alienController.LockerRoomSequence();
                break;
            case "Radio Transmitter":
                helptextForRadio.PressTAB = true;
                ButtonForRadio.off_until_special = false;
                TriggerForCockpit.SetActive(true);
                break;
            case "Purple Access Key":
                DarkFigureForPurpleKey.spawnFigure();
                AlienAttentionHandler.SetTo100AtLocation(player.transform.position);
                break;
            case "Sticky Note":
                DarkFigureForPurpleKey.spawnFigure();
                AlienAttentionHandler.SetTo100AtLocation(player.transform.position);
                break;
            default:
                break;
        }

        if (!item.isItem)
        {
            JournalPlayer.GetComponent<PlayJournal>().PlayAudioOnPickUp(item);
        }
            

        pickUp();


        //ui_controller.uiInvetory.RefreshInventoryItems();

    }

    public void pickUp()
    {
        OnPickUp();
        gameObject.SetActive(false);
        if (item.isItem)
        {
            ui_controller.inventory.AddItem(item);
        }
        else
        {
            ui_controller.inventory.AddJournal(item);
        }
        ItemGlow.SetActive(false);
    }

    public void deletedPickUp()
    {
        OnPickUp();
        gameObject.SetActive(false);
        ItemGlow.SetActive(false);
    }

    public override void Load(JObject state)
    {
        /*
        var active = (bool)state[fullName]["isActive"];
        gameObject.SetActive(active);
        ItemGlow.SetActive(active);
        base.Load(state); */
    }

    public override void Save(ref JObject state)
    {
        /*
        base.Save(ref state);
        state[fullName]["isActive"] = gameObject.activeSelf; */
    }

    void OnPickUp()
    {
        ItemPickedUp?.Invoke(this, new PickUpEventArgs(gameObject));
    }
}

public class PickUpEventArgs : EventArgs
{
    public GameObject pickUpItem;
    public PickUpEventArgs(GameObject pickUpItem)
    {
        this.pickUpItem = pickUpItem;
    }
}
