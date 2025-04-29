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
    public AudioSource blendAudio;
    public AudioSource ding;
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
        StartCoroutine(SpawnVial());
    }

    public void closeVat(bool spawnFromSave)
    {
        vatClose.SetTrigger("closed");
        if(!spawnFromSave)
            StartCoroutine(PlayAudio());
        //vatCloseAudio.Play();
    }

    IEnumerator SpawnVial()
    {
        yield return new WaitForSeconds(2f);
        visualVial.SetActive(false);
        pickUpVial.SetActive(true);
    }
    IEnumerator PlayAudio()
    {
        vatCloseAudio.Play();
        yield return new WaitForSeconds(2f);
        blendAudio.Play();
        yield return new WaitForSeconds(10f);
        ding.Play();
    }
}
