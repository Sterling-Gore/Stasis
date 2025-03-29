using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingManTrigger : MonoBehaviour
{
    public Animator HangingMan;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HangingMan.SetTrigger("active");
            gameObject.SetActive(false);

        }
    }

}
