using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceAudio : MonoBehaviour
{
    public UI_Controller UIcontroller;

    [Header("Ambiance audios")]
    public AudioSource ambiance;
    public AudioSource VHSstatic;
    UI_Controller.UI_Types Current_UI;
    // Start is called before the first frame update
    void Start()
    {
        ambiance.Pause();
        VHSstatic.Pause();
        VHSstatic.ignoreListenerPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(UIcontroller.UI_Value == UI_Controller.UI_Types.escape_menu || UIcontroller.UI_Value == UI_Controller.UI_Types.options_menu)
        {
            VHSstatic.UnPause();
            ambiance.Pause();
        }
        else
        {
            ambiance.UnPause();
            VHSstatic.Pause();
        }
        //VHSstatic.Play();
    }
}
