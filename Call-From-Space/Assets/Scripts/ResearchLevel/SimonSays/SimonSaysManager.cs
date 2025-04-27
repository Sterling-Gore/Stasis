using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysManager : MonoBehaviour
{
    [Header("Manager")]
    public bool puzzleIsCompleted = false;

    [Header("GameObjects")]
    public GameObject screenSparkle;
    public GameObject antidoteDeposit;
    public GameObject emptyCanister;
    public GameObject filledCanister;

    [Header("screen")]
    public SimonSaysScreemInteraction screen;

    // Start is called before the first frame update
    void Start()
    {
        if(puzzleIsCompleted)
        {
            screen.finished = true;
            screenSparkle.SetActive(false);
            antidoteDeposit.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void emptyTheCanister()
    {
        filledCanister.SetActive(false);
        emptyCanister.SetActive(true);
    }

    public void completePuzzle()
    {
        puzzleIsCompleted = true;
        emptyTheCanister();
    }
}
