using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoToggle :  Interactable
{
    public VideoPlayer videoPlayer;
    public bool isOn = false;
    private bool wasPlayingBeforePause = false;
    public viewingScreen viewing;
    // Start is called before the first frame update
    void Start()
    {
        if(!isOn)
        {
            videoPlayer.Stop();
            //videoPlayer.enabled = false; // Disables the video player
        }
        else
        {
            videoPlayer.frame = 0;       // Resets the video to the beginning
            videoPlayer.Play();
        }
    }
    private void Update()
    {
        // Check if the game is paused
        if (Time.timeScale == 0f && isOn && !viewing.isWatching)
        {
            videoPlayer.Pause();
            wasPlayingBeforePause = true;
        }
        else if (Time.timeScale > 0f && isOn && wasPlayingBeforePause)
        {
            videoPlayer.Play();
            wasPlayingBeforePause = false;
        }
    }

    public override string GetDescription()
    {
        if(!isOn)
            return ("<color=red>Press [E]</color=red> to Play the Tape");
        else
            return ("<color=red>Press [E]</color=red> to Stop the Tape");
        return ("");
    }

    public override void Interact()
    {
        isOn = !isOn;
        if(!isOn)
        {
            videoPlayer.Stop();
            //videoPlayer.enabled = false; // Disables the video player
        }
        else
        {
            videoPlayer.frame = 0;       // Resets the video to the beginning
            videoPlayer.Play();
        }
    }
}
