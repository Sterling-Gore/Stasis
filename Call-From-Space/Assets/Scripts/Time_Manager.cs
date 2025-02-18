using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_Manager : MonoBehaviour
{
    public UI_Controller ui_controller;
    UI_Controller.UI_Types Current_UI;
    // Start is called before the first frame update
    void Start()
    {
        Current_UI = ui_controller.UI_Value;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Current_UI != ui_controller.UI_Value)
        {
            Current_UI = ui_controller.UI_Value;
            if( Current_UI ==  UI_Controller.UI_Types.escape_menu || Current_UI ==  UI_Controller.UI_Types.options_menu)
                Time.timeScale = 0f;
            else   
                Time.timeScale = 1f;
        }
    }
}
