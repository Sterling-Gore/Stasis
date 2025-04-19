using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabScreenPuzzle : MonoBehaviour
{
    public GameObject[] buttons;
    public Sprite[] symbols;

    public int[] solution;
    public GameObject[] current_answer_visual;
    public int[] current_answer_number;
    int current_solution_slot = 0;

    public bool won;

    [Header("Audios")]
    public AudioClip enableSound;
    public AudioClip disableSound;
    public AudioClip selectBox;
    public AudioClip unSelectBox;
    public AudioClip valid;
    public AudioClip inValid;
    public AudioSource audioSource;

    [Header("GenScreen")]
    public labScreenInteraction LabScreen;
    public UI_Controller PlayerUI;
    public GameObject player;


    [Header("Colors")]
    public Color red;
    public Color green;
    public Color grey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable() 
    {
        if (enableSound != null)
        {
            audioSource.PlayOneShot(enableSound);
        }
        if(!won)
        {
            TurnInteractableButtons(true);
            updateSolutionScreen();
        }
    }

    void OnDisable() 
    {
        // Play the disable sound if assigned
        Debug.Log("OnDisable called");
        if (disableSound != null)
        {
            Debug.Log("PLAY DISABLE SOUND");
            audioSource.PlayOneShot(disableSound);
        }
    }

    void TurnInteractableButtons(bool enable){
        for(int i = 0; i < buttons.Length; i++){
            buttons[i].GetComponent<Button>().interactable = enable;
            buttons[i].transform.GetComponent<UnityEngine.UI.Image>().color = grey;
        }
    }

    public void symbolButtonClick(int buttonNumber)
    {
        if(!won)
        {
            if(current_solution_slot < 5)
            {
                current_answer_number[current_solution_slot] = buttonNumber;
                current_solution_slot += 1;
                updateSolutionScreen();
                audioSource.PlayOneShot(selectBox);
            }   
        }
    }

    public void backButtonClick()
    {
        if(!won)
        {
            if(current_solution_slot > 0)
            {
                current_solution_slot -= 1;
                current_answer_number[current_solution_slot] = -1;
                updateSolutionScreen();
                audioSource.PlayOneShot(unSelectBox);
            }
        }
    }

    public void submitButtonClick()
    {
        if(!won)
        {
            bool match = true;
            for(int i = 0; i < current_answer_number.Length; i++){
                if(current_answer_number[i] != solution[i])
                {
                    match = false;
                }
            }

            if(match)
            {
                won = true;
                LabScreen.finishPuzzle();
                StartCoroutine(GreenOrder());
            }
            else
            {
                StartCoroutine(RedOrder());
            }
        }
    }


    void updateSolutionScreen()
    {
        for(int i = 0; i < current_answer_number.Length; i++){
            if(current_answer_number[i] == -1)
            {
                current_answer_visual[i].SetActive(false);
            }
            else
            {
                current_answer_visual[i].SetActive(true);
                current_answer_visual[i].transform.GetComponent<UnityEngine.UI.Image>().sprite = symbols[current_answer_number[i]];
            }
            //buttons[i].transform.GetComponent<UnityEngine.UI.Image>().color = grey;
        }
        //this will update the solution screen
    }



    IEnumerator RedOrder(){
        TurnInteractableButtons(false);
        

        yield return new WaitForSeconds(0.25F);
        for(int j = 0; j < 3; j++){
            audioSource.PlayOneShot(inValid);

        for(int i = 0; i < buttons.Length; i++){
            //buttonsOff[i].transform.GetComponent<UnityEngine.UI.Image>().sprite = redButton;
            buttons[i].transform.GetComponent<UnityEngine.UI.Image>().color = red;
        }
        yield return new WaitForSeconds(0.5F);
        for(int i = 0; i < buttons.Length; i++){
        //buttonsOff[i].transform.GetComponent<UnityEngine.UI.Image>().sprite = NOTHING;
        buttons[i].transform.GetComponent<UnityEngine.UI.Image>().color = grey;
        }
         yield return new WaitForSeconds(0.25F);
        }

       TurnInteractableButtons(true);
    }


    IEnumerator GreenOrder(){
        
        yield return new WaitForSeconds(0.25F);
        
        for(int j = 0; j < 5; j++){
            audioSource.PlayOneShot(valid);

        for(int i = 0; i < buttons.Length; i++){
            //buttonsOff[i].transform.GetComponent<UnityEngine.UI.Image>().sprite = greenButton;
            buttons[i].transform.GetComponent<UnityEngine.UI.Image>().color = green;
        }
        yield return new WaitForSeconds(0.5F);
        for(int i = 0; i < buttons.Length; i++){
        //buttonsOff[i].transform.GetComponent<UnityEngine.UI.Image>().sprite = NOTHING;
        buttons[i].transform.GetComponent<UnityEngine.UI.Image>().color = grey;
        }
         yield return new WaitForSeconds(0.25F);
        }
        if(PlayerUI.UI_Value  == UI_Controller.UI_Types.inventory_or_puzzle)
            PlayerUI.ESCAPE();

        
       
    }
}
