using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Controller : MonoBehaviour
{
    [Header("Puzzle UI")]
    public GameObject[] PuzzleUIs

    [Header("Inventory and Map UI")]
    public GameObject Inventory_and_Map_UI;
    public GameObject Inventory_UI_Object;
    public GameObject Map_UI_Object;
    public GameObject Inspector_UI_Object;
    public GameObject TaskList_UI_Object;
    public UI_Inventory uiInventory;
    public Inventory inventory;
    private bool showInventory;

    [Header("Screens UI")]
    public GameObject standardScreen;



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
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }


    void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //open inventory
        }
    }
}
