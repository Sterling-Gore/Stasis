using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Map : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip enableSound;
    public AudioSource audioSource;
    // Start is called before the first frame update
  

    void OnEnable()
    {
        audioSource.PlayOneShot(enableSound);
    }
}
