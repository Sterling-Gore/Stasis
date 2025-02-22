using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Dot_ShipConsole : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image dot;
    public Color glowColor = new Color32(8, 248, 255, 255);
    public float glowIntensity = 1.5f;
    private Color originalColor;
    public Dot_Grid ParentScript;
    public ShipConsolePuzzle PuzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        if (dot == null)
        {
            dot = GetComponent<Image>();
        }
        originalColor = dot.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!PuzzleManager.finished)
        {
            dot.color = glowColor * glowIntensity;
            PuzzleManager.PlayHover();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!PuzzleManager.finished)
        {
            dot.color = originalColor;
        }
    }

    public void OnClick(int dot_num)
    {
        if(!PuzzleManager.finished)
        {
            if(ParentScript.isDrawing)
                ParentScript.Finish_Line(dot_num);
            else
                ParentScript.Start_Line(dot_num);
            PuzzleManager.PlayClick();
        }
    }
}
