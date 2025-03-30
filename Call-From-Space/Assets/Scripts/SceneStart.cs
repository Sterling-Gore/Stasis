using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        AudioListener.pause = true;
    }
    void Start()
    {
        Time.timeScale = 1f;
        //AudioListener.volume = 100f;
        //AudioListener.pause = false;
        StartCoroutine(DelayAudio());
    }

    IEnumerator DelayAudio()
    {
        yield return new WaitForSeconds(.5f);
        AudioListener.pause = false;
    }

}
