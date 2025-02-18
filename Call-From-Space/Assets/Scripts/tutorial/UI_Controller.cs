using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [Header("Puzzle UI")]
    public GameObject[] PuzzleUIs;

    [Header("Inventory and Map UI")]
    public GameObject Inventory_and_Map_UI;
    public GameObject Inventory_UI_Object;
    public GameObject Map_UI_Object;
    public GameObject Inspector_UI_Object;
    public GameObject TaskList_UI_Object;
    
    [Header("Inventory Data")]
    public UI_Inventory uiInventory;
    public Inventory inventory;

    private bool showInventory;

    [Header("Screens UI")]
    public GameObject astronautOverlay;
    public GameObject preSuitScreen;
    public GameObject pauseMenuUI;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject standardScreen;

    [Header("Interactor")]
    Interactor interactor;
    public bool IsSpaceSuitOn;




    public enum UI_Types
    {
        inspector,
        inventory_or_puzzle,
        standard,
        escape_menu,
        options_menu
    }

    public UI_Types UI_Value;

    // Start is called before the first frame update
    void Start()
    {
        UI_Value = UI_Types.standard;
        inventory = new Inventory();
        uiInventory = Inventory_UI_Object.GetComponent<UI_Inventory>();
        uiInventory.setInventory(inventory);

        interactor = gameObject.GetComponent<Interactor>();
        astronautOverlay.SetActive(IsSpaceSuitOn);
        preSuitScreen.SetActive(!IsSpaceSuitOn);
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }


    void MyInput()
    {
        //toggles inventory
        if (Input.GetKeyDown(KeyCode.Tab) && IsSpaceSuitOn)
        {
            if(UI_Value == UI_Types.standard)
            {
                Set_UI_Value(UI_Types.inventory_or_puzzle);
                Inventory_and_Map_UI.SetActive(true);
            }
            else if(UI_Value == UI_Types.inventory_or_puzzle)
            {
                Set_UI_Value(UI_Types.standard);
                Inventory_and_Map_UI.SetActive(false);
            }

        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ESCAPE();
        }

    }

    public void PutOnSuit()
    {
        astronautOverlay.SetActive(true);
        preSuitScreen.SetActive(false);
        IsSpaceSuitOn = true;
    }

    public void ESCAPE()
    {
        if (UI_Value == UI_Types.inspector)
        {
            Set_UI_Value(UI_Types.inventory_or_puzzle);
            Inspector_UI_Object.GetComponent<Inspector>().unloadInspector();
            Inventory_and_Map_UI.SetActive(true);
        }
        else if (UI_Value == UI_Types.inventory_or_puzzle)
        {
            Set_UI_Value(UI_Types.standard);
            Inventory_and_Map_UI.SetActive(false);
            foreach ( GameObject Puzzle in PuzzleUIs)
            {
                Puzzle.SetActive(false);
            }
        }
        else if (UI_Value == UI_Types.standard)
        {
            Set_UI_Value(UI_Types.escape_menu);
            pauseMenuUI.SetActive(true);
        }
        else if (UI_Value == UI_Types.escape_menu)
        {
            Set_UI_Value(UI_Types.standard);
            pauseMenuUI.SetActive(false);
        }
        else if (UI_Value == UI_Types.options_menu)
        {
            Set_UI_Value(UI_Types.escape_menu);
            controlsMenu.SetActive(false);
            optionsMenu.SetActive(false);
        }
    }


    public void Set_UI_Value(UI_Types value)
    {
        UI_Value = value;
        if (UI_Value == UI_Types.standard) 
            ToggleStandardScreen(true);
        else
            ToggleStandardScreen(false);
    }
//------------------------------------------------------
    public void OnClickInspector()
    {
        Set_UI_Value(UI_Types.inspector);
    }

    public void OnClickInventory_or_Puzzle()
    {
        Set_UI_Value(UI_Types.inventory_or_puzzle);
    }

    public void OnClickStandard()
    {
        Set_UI_Value(UI_Types.standard);
    }

    public void OnClickEscape_Menu()
    {
        Set_UI_Value(UI_Types.escape_menu);
    }

    public void OnClickOptions_Menu()
    {
        Set_UI_Value(UI_Types.options_menu);
    }

//--------------------------------------------------------
    public void ToggleStandardScreen(bool turnOn)
    {
        if (turnOn)
        {
            standardScreen.SetActive(true);
            //TaskList_UI_Object.transform.Find("TaskContainer").gameObject.SetActive(true);
            //TaskList_UI_Object.GetComponent<TaskList>().Refresh();
            interactor.inUI = false;
        }
        else
        {
            standardScreen.SetActive(false);
            //TaskList_UI_Object.transform.Find("TaskContainer").gameObject.SetActive(false);
            interactor.inUI = true;
        }
    }
}
