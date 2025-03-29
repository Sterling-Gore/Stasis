using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpTexts : MonoBehaviour
{
    public TextMeshProUGUI text;
    [Header("Tutorial")]
    public bool PressTAB = false;
    public bool PressF = false;
    public bool PressCTRL = false;

    [Header("Workshop")]
    public bool NewMapAdded = false;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left ctrl"))
        {
            PressCTRL = false;
        }
        else if(Input.GetKeyUp(KeyCode.F))
        {
            PressF = false;
        }
        else if(Input.GetKeyDown(KeyCode.Tab))
        {
            PressTAB = false;
            NewMapAdded = false;
        }


        if(PressTAB)
        {
            text.text = "<color=red>Press [TAB]</color=red> to open inventory and map";
        }
        else if(PressF)
        {
            text.text = "<color=red>Press [F]</color=red> to toggle flashlight";
        }
        else if(PressCTRL)
        {
            text.text = "<color=red>Press [LEFT CTRL]</color=red> to toggle crouch";
        }
        else if(NewMapAdded)
        {
            text.text = "<color=red>Press [TAB]</color=red> to view new map";
        }
        else
        {
            text.text = "";
        }
    }
}
