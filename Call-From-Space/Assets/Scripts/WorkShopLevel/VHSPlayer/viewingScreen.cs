using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewingScreen :  Interactable
{
    public Transform viewingPosition; // Assign a transform where the camera should move when watching
    public Camera playerCamera; // Reference to the player's main camera

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public bool isWatching = false;

    public GameObject viewingUI;
    public GameObject astronautOverlay;
    public GameObject player;
    public MoveCamera movecamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isWatching)
        {
            playerCamera.transform.position = originalPosition;
            playerCamera.transform.rotation = originalRotation;
            isWatching = false;
            movecamera.active = true;


        }
    }

    public override string GetDescription()
    {
        return("<color=red>Press [E]</color=red> to Watch the Screen");
        /*
        if(!isOn)
            return ("<color=red>Press [E]</color=red> to Play the Tape");
        else
            return ("<color=red>Press [E]</color=red> to Stop the Tape");
        return ("");*/
    }

    public override void Interact()
    {
        movecamera.active = false;
        viewingUI.SetActive(true);
        astronautOverlay.SetActive(false);
        player.GetComponent<Interactor>().inUI = true;
        player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.viewing_screen);

        originalPosition = playerCamera.transform.position;
        originalRotation = playerCamera.transform.rotation;

        // Move camera to the viewing position
        playerCamera.transform.position = viewingPosition.position;
        playerCamera.transform.rotation = viewingPosition.rotation;
        isWatching = true;

    }
}
