using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverideElectircPanel : Interactable
{
    private bool waitingForKey = false; 
    public SpikeStabber spikeStabber;


    public GameObject Sparkle;
    public GameObject panelLightning;
    public bool PuzzleCompleted = false;
    public GameObject PuzzleUI;
    public GameObject player;
    bool breakTheRoutine = false;
    public bool isFinal;
    public float waitBetweenRounds = 1f;
    public float timeLimit = 5f; // Time in seconds for the player to respond

    [Header("Audios")]
    public AudioSource PlantScreech;
    public AudioSource LightningBolt;
    public AudioSource AudioSource;
    public AudioClip Invalid;
    public AudioClip Valid;
    public AudioSource FinalScreech;

    [Header("Camera Shake")]
    public CameraShakeGeneral cameraShake;

    [Header("Visuals")]
    public Sprite[] keySprites;
    public string[] Keycodes;
    public GameObject ButtonMashImage;

    [Header("Vine Burner")]
    public VineBurner vineBurner;

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            {
                breakTheRoutine = true;
            }
    }

    public override string GetDescription()
    {
        if(!PuzzleCompleted)
            return ("Press [E] to Override Electrical Panel");
        else
            return ("");
    }

    public override void Interact()
    {
        if(!PuzzleCompleted)
        {
            breakTheRoutine = false;
            PuzzleUI.SetActive(true);
            ButtonMashImage.SetActive(false);
            player.GetComponent<Interactor>().inUI = true;
            player.GetComponent<UI_Controller>().Set_UI_Value(UI_Controller.UI_Types.inventory_or_puzzle);
            StartCoroutine(StartButtonMash());
        }
    }


    IEnumerator StartButtonMash()
    {
        bool flag = true;
        int count = 0;
        while (flag)
        {
            // Generate a random key
            //randomKey = GetRandomKey();
            //Debug.Log("Press the key: " + randomKey);
            if(breakTheRoutine)
                yield break;
            yield return new WaitForSeconds(waitBetweenRounds); // Short delay before next round

            waitingForKey = true;
            float timer = 0f;

            int randnum = Random.Range(0,keySprites.Length);
            ButtonMashImage.SetActive(true);
            ButtonMashImage.transform.GetComponent<Image>().sprite = keySprites[randnum]; 
            ButtonMashImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-450,450), Random.Range(-200,150));

            // Wait for the correct key or timeout
            while (timer < timeLimit)
            {
                if (Input.GetKeyDown(Keycodes[randnum]))
                {
                    Debug.Log("Success! You pressed the correct key: ");
                    count += 1;
                    waitingForKey = false;
                    AudioSource.PlayOneShot(Valid);
                    break;
                }
                else if(Input.anyKeyDown)
                {
                    Debug.Log("Fail! You pressed the wrong key: ");
                    waitingForKey = false;
                    count = 0;
                    AudioSource.PlayOneShot(Invalid);
                    cameraShake.StartShake(.1f, 0.01f);
                    break;
                }

                if(breakTheRoutine)
                    yield break;

                timer += Time.deltaTime;
                yield return null;
            }

            if (waitingForKey)
            {
                Debug.Log("Time's up! You failed to press the correct key.");
                count = 0;
                AudioSource.PlayOneShot(Invalid);
                cameraShake.StartShake(.1f, 0.01f);
            }

            ButtonMashImage.SetActive(false);
            if (count >= 4)
            {
                flag = false;
                PuzzleCompleted = true;
                Sparkle.SetActive(false);
                finishedPuzzle();
                player.GetComponent<UI_Controller>().ESCAPE();
            }
        }
    }

    void finishedPuzzle()
    {
        panelLightning.SetActive(false);
        vineBurner.startBurn();
        if(isFinal)
            FinalScreech.Play();
        else
            PlantScreech.Play();
        LightningBolt.Play();
        cameraShake.StartShake(2f, 0.05f);

        spikeStabber.spikeCoolDownTimer -= 2.33f;
        spikeStabber.initialRumbleWaitPeriod *= .25f;
        spikeStabber.underPlayerWaitPeriod -= .5f;
    }
    
}
