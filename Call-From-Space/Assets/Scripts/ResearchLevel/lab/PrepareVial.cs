using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareVial : MonoBehaviour
{
    public GameObject pickUpVial;
    public GameObject visualVial;
    public AudioSource liquidFillingUp;

    [Header("vat")]
    public Animator vatClose;
    public AudioSource vatCloseAudio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createVial()
    {
        liquidFillingUp.Play();
        visualVial.SetActive(false);
        pickUpVial.SetActive(true);
    }

    public void closeVat()
    {
        vatClose.SetTrigger("closed");
        vatCloseAudio.Play();
    }
}
