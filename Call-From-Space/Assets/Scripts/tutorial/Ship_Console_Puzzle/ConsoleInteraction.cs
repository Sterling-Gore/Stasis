using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConsoleInteraction : Interactable
{

    public GameObject ConsoleUI;
    public GameObject player;
    public Interactor interactor;
    public bool IsAvailable;
    public bool Finished;
    bool flying = false;
    bool JustFinished = false;

    public CameraShakeGeneral cameraShake;

    [Header("Sparkle")]
    public GameObject sparkle;

    [Header("AI Sounds")]
    public AI_Tutorial_Sounds AI_Sounds;

    [Header("Game Manager")]
    public SaveManager saveManager;


    

    // Start is called before the first frame update
    void Start()
    {
        IsAvailable = false;
        Finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(JustFinished != Finished)
        {
            JustFinished = true;
            AI_Sounds.PlayStasisDestination();
        }
    }

    public override string GetDescription()
    {
        if(flying)
        {
            return("");
        }
        else if (Finished)
            return ("<color=red>Press [E]</color=red> to Start Your Ship");
        else if(IsAvailable)
            return ("<color=red>Press [E]</color=red> to Access the Flightdeck Console");
        return ("Flightdeck Console is Offline, Restore Power to Engine");
    }

    public override void Interact()
    {
        if (Finished && !flying)
        {
            sparkle.SetActive(false);
            AI_Sounds.PlayAutopilotEnganged();
            flying = true;
            StartCoroutine(ShipFlying());
        }
        else if(IsAvailable)
        {
            ConsoleUI.SetActive(true);
            player.GetComponent<Interactor>().inUI = true;
            player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.inventory_or_puzzle);
        }
    }

    IEnumerator ShipFlying()
    {
        yield return new WaitForSeconds(6f);
        cameraShake.StartShake(13f, 0.1f);
        yield return new WaitForSeconds(11f);
        interactor.inUI = true;
        saveManager.UpdateSave(SavePointID.tutorial);
        SceneManager.LoadSceneAsync("Workshop");
    }
}
