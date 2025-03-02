using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDownWarningTrigger : MonoBehaviour
{
    [Header("AI Sounds")]
    public AI_Tutorial_Sounds AI_Sounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        AI_Sounds.PlayWarnLockDown();
        gameObject.SetActive(false);
    }
}
