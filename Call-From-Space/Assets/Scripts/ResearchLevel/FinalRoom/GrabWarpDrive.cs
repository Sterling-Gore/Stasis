using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrabWarpDrive : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string GetDescription()
    {
        return ("Press [E] to pick up Warp Drive");
    }

    public override void Interact()
    {
        SceneManager.LoadScene("TOBECONTINUED");
    }
}
