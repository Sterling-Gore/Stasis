using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysManager : MonoBehaviour
{
    [Header("Manager")]
    public bool puzzleIsCompleted = false;
    public BasementFlickeringLights lights;

    [Header("GameObjects")]
    public GameObject screenSparkle;
    public GameObject antidoteDeposit;
    public GameObject emptyCanister;
    public GameObject filledCanister;

    [Header("screen")]
    public SimonSaysScreemInteraction screen;

    [Header("InfiniteHallway")]
    public GameObject savepointCollider;
    public GameObject teleportCollider;
    public GameObject vcr;

    [Header("Dark Figure")]
    public DarkFigureController darkFigure;
    public FakeItemSpawner fakeItems;

    // Start is called before the first frame update
    void Start()
    {
        if(puzzleIsCompleted)
        {
            screen.finished = true;
            screenSparkle.SetActive(false);
            antidoteDeposit.SetActive(false);
            savepointCollider.SetActive(false);
            teleportCollider.SetActive(false);
            vcr.SetActive(true);
            lights.CompleteLights();
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
        darkFigure.SendToPermanentTimeout();
        fakeItems.deleteAllFakes();
        StartCoroutine(delayBetweenCompletion());
    }

    IEnumerator delayBetweenCompletion()
    {
        yield return new WaitForSeconds(2f);
        emptyTheCanister();
        teleportCollider.SetActive(false);
        savepointCollider.SetActive(true);
        vcr.SetActive(true);
        lights.CompleteLights();
    }
}
