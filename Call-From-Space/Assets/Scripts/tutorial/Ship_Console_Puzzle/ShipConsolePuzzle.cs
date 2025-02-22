using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipConsolePuzzle : MonoBehaviour
{
    // Start is called before the first frame update
    int[ , ] SolutionA = { 
        { 0, 0, 0, 0, 0, 1, 0, 1, 0}, 
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 1, 0, 0, 0},
        { 1, 0, 0, 0, 1, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 1, 0},
        { 1, 0, 0, 0, 0, 0, 1, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    int[ , ] SolutionB = { 
        { 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
        { 0, 0, 0, 0, 0, 0, 1, 0, 1},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 1, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 1, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 1, 0, 0, 0, 1},
        { 0, 1, 0, 0, 0, 0, 0, 1, 0}
    };
    int[ , ] SolutionC = { 
        { 0, 1, 0, 1, 0, 0, 0, 0, 0}, 
        { 1, 0, 1, 0, 0, 0, 0, 0, 0},
        { 0, 1, 0, 0, 0, 0, 0, 0, 0},
        { 1, 0, 0, 0, 0, 0, 0, 1, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 1, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 1, 0, 1, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    public Dot_Grid GridLeft;
    public Dot_Grid GridMiddle;
    public Dot_Grid GridRight;


    public AudioClip enableSound;
    public AudioClip disableSound;

    public AudioClip Hover;
    public AudioClip Click;

    public AudioClip valid;

    public AudioClip inValid;

    public AudioClip Clear;

    public AudioSource audioSource;

    public Color red = new Color32(8, 248, 255, 255);
    public Color green = new Color32(8, 248, 255, 255);
    public Color white = new Color32(8, 248, 255, 255);

    public bool finished;
    public ConsoleInteraction interactor;
    public UI_Controller PlayerUI;
    void Start()
    {
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleSubmit()
    {

        //THIS IS FOR DEBUGING
        /*
        for(int i = 0; i < 9; i++)
        {
            string text = "";
            for (int j = 0; j < 9; j++)
            {
                text = text + " " + GridLeft.CurrentSolution[i,j].ToString();
            }
            Debug.Log(text);
        }
        */
        if( CompareSolutions())
        {
            if (valid != null)
            {
                audioSource.PlayOneShot(valid);
            }
            Debug.Log("Success");
            StartCoroutine(Green());
            finished = true;
            interactor.Finished = true;
        }
        else
        {
            if (inValid != null)
            {
                audioSource.PlayOneShot(inValid);
            }
            Debug.Log("Incorrect");
            StartCoroutine(Red());
        }


    }


    void OnEnable() 
    {
        ChangeColor(white);
        if (enableSound != null)
        {
            audioSource.PlayOneShot(enableSound);
        }
    }
    void OnDisable() 
    {
        if (disableSound != null)
        {
            audioSource.PlayOneShot(disableSound);
        }
    }
    public void PlayHover()
    {
        if (Hover != null)
        {
            audioSource.PlayOneShot(Hover);
        }
    }
    public void PlayClick()
    {
        if (Click != null)
        {
            audioSource.PlayOneShot(Click);
        }
    }
    public void PlayClear()
    {
        if (Clear != null)
        {
            audioSource.PlayOneShot(Clear);
        }
    }

    bool CompareSolutions()
    {
        for(int i = 0; i < 9; i++)
        {
            
            for (int j = 0; j < 9; j++)
            {
                if(GridLeft.CurrentSolution[i,j] != SolutionA[i,j])
                    return false;
                if(GridMiddle.CurrentSolution[i,j] != SolutionB[i,j])
                    return false;
                if(GridRight.CurrentSolution[i,j] != SolutionC[i,j])
                    return false;
            }
            
        }
        return true;
    }




    IEnumerator Green()
    {
        ChangeColor(green);

        yield return new WaitForSeconds(0.5F);

        PlayerUI.ESCAPE();

        //ChangeColor(white);
    }

    IEnumerator Red()
    {
        
        ChangeColor(red);

        yield return new WaitForSeconds(0.5F);

        ChangeColor(white);


    }

    void ChangeColor(Color newColor)
    {
        foreach (Transform line in GridLeft.AllLines)
        {
            line.GetComponent<Image>().color = newColor;
        }
        foreach (Transform line in GridMiddle.AllLines)
        {
            line.GetComponent<Image>().color = newColor;
        }
        foreach (Transform line in GridRight.AllLines)
        {
            line.GetComponent<Image>().color = newColor;
        }

        foreach (RectTransform dot in GridLeft.dots)
        {
            dot.transform.GetComponent<Image>().color = newColor;
        }
        foreach (RectTransform dot in GridMiddle.dots)
        {
            dot.transform.GetComponent<Image>().color = newColor;
        }
        foreach (RectTransform dot in GridRight.dots)
        {
            dot.transform.GetComponent<Image>().color = newColor;
        }
    }


}
