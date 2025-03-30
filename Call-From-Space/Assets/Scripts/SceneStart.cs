using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    // Start is called before the first frame update
    public bool delayAudio = false;
    void Awake()
    {
        AudioListener.pause = true;
    }
    void Start()
    {
        Time.timeScale = 1f;
        //AudioListener.volume = 100f;
        //AudioListener.pause = false;
        if(delayAudio)
            StartCoroutine(DelayAudio());
        else
            AudioListener.pause = false;
    }

    IEnumerator DelayAudio()
    {
        yield return new WaitForSeconds(.5f);
        AudioListener.pause = false;
    }

}
