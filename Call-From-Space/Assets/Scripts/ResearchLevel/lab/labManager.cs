using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class labManager : MonoBehaviour
{
    public bool completed = false;
    public UI_Controller player_ui;
    public SaveManager saveManager;

    [Header("Deposits")]
    public plant_deposit plant_depo;
    public ValveDeposit valve_depo;

    [Header("Screens")]
    public labScreenInteraction plantLabScreen;
    public labScreenInteraction bloodLabScreen;

    [Header("Valves")]
    public lab_valve_interaction plantValve;
    public lab_valve_interaction bloodValve; 

    [Header("Vials")]
    public Item plantVial;
    public Item bloodVial;

    [Header("Doors")]
    public Animator plantDoorAnimator;
    public Animator bloodDoorAnimator;
    public AudioSource plantDoorAudio;
    public AudioSource bloodDoorAudio;
    // Start is called before the first frame update

    public void complete_for_awake()
    {
        //completed = true;
        
        plant_depo.deposit();
        valve_depo.deposit();

        plantLabScreen.finishPuzzle();
        bloodLabScreen.finishPuzzle();

        plantValve.isComplete = true;
        bloodValve.isComplete = true;
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!completed)
        {
            if(player_ui.inventory.IsItemInList(plantVial) && player_ui.inventory.IsItemInList(bloodVial))
            {
                finishPuzzle();
            }
        }
    }


    public void finishPuzzle()
    {
        completed = true;
        saveManager.UpdateSave(SavePointID.research2);
        plantDoorAnimator.SetTrigger("Open");
        plantDoorAudio.Play();
        bloodDoorAnimator.SetTrigger("Open");
        bloodDoorAudio.Play();
    }
}
