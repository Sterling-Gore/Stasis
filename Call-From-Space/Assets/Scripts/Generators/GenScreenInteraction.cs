using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenScreenInteraction : Interactable
{

    public enum Generator{
        A,
        B,
        C
    }
    public GameObject GenUI;
    public GameObject player;


    public Generator generatorType;

    public SaveManager saveManager;
    public bool finished;

    [Header("Generator Components")]
    public GameObject ScreenSparkle;
    public GameObject FuelDepositSparkle;
    public Animator genDoorAnimator;
    public Collider FuelDepositCollider;
    public FuelCellHoldable FuelCell;
    public AudioSource genRepeatingAudio;
    public GameObject particles;

    [Header("PowerManager")]
    public PowerManager powerManager;

    void Awake()
    {
        SavePointID savePoint = saveManager.LoadSave();
        switch (savePoint)
        {
            case SavePointID.workshop1:
                finished = false;
                break;
            case SavePointID.workshop2:
                finished = false;
                break;
            case SavePointID.workshop3:
                if(generatorType == Generator.A)
                {
                    FinishPuzzle();
                    FinishGenerator(true);
                }
                break;
            case SavePointID.workshop4:
                FinishPuzzle();
                FinishGenerator(true);
                break;
            default:
                finished = false;
                break;
        }
    }
    public override string GetDescription()
    {
        
        return ("Press [E] to interact with the Generator Screen");
    }

    public override void Interact()
    {
        switch (generatorType)
        {
            case Generator.A:
                //player.GetComponent<PlayerController>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle1(1);
                break;
            case Generator.B:
                //player.GetComponent<PlayerController>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle2(5);
                break;
            case Generator.C:
                //player.GetComponent<PlayerController>().TaskList_UI_Object.GetComponent<TaskList>().GenPuzzle3(1);
                break;
            default:
                break;

        }
        //GenUI.GetComponent<GeneratorGame>().interactor.inUI = true;
        GenUI.SetActive(true);
        player.GetComponent<Interactor>().inUI = true;
        player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.inventory_or_puzzle);
        //player.GetComponent<PlayerController>().Set_UI_Value(1);
        
    }

    public void FinishPuzzle()
    {
        finished = true;
        ScreenSparkle.SetActive(false);
        FuelDepositSparkle.SetActive(true);
        genDoorAnimator.SetTrigger("Open");
        FuelDepositCollider.enabled = true;  
        gameObject.GetComponent<Collider>().enabled = false;
    }

    public void FinishGenerator(bool FromSavePoint = false)
    {
        FuelDepositSparkle.SetActive(false);
        FuelDepositCollider.enabled = false; 
        genDoorAnimator.SetTrigger("Closed"); 
        FuelCell.deposit();
        if(FromSavePoint)
        {
            genRepeatingAudio.enabled = true;
            particles.SetActive(true);
        }

        if(generatorType == Generator.A)
        {
            powerManager.turnOnGenA();
            if(!FromSavePoint)
            {
                saveManager.UpdateSave(SavePointID.workshop3);
            }
        }
        else
        {
            powerManager.turnOnGenB();
            if(!FromSavePoint)
            {
                saveManager.UpdateSave(SavePointID.workshop4);
            }
        }
        
    }
}
