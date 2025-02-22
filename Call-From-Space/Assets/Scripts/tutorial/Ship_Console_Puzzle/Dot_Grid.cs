using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dot_Grid : MonoBehaviour
{
    //public int[][] CurrentSolution;
    public int[ , ] CurrentSolution = { 
        { 0, 0, 0, 0, 0, 0, 0, 0, 0}, 
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0},
        { 0, 0, 0, 0, 0, 0, 0, 0, 0}
        };
    
    public int[ , ] ValidEdges = { 
        /*1*/{ 0, 1, 0, 1, 1, 1, 0, 1, 0}, 
        /*2*/{ 1, 0, 1, 1, 1, 1, 1, 0, 1},
        /*3*/{ 0, 1, 0, 1, 1, 1, 0, 1, 0},
        /*4*/{ 1, 1, 1, 0, 1, 0, 1, 1, 1},
        /*5*/{ 1, 1, 1, 1, 0, 1, 1, 1, 1},
        /*6*/{ 1, 1, 1, 0, 1, 0, 1, 1, 1},
        /*7*/{ 0, 1, 0, 1, 1, 1, 0, 1, 0},
        /*8*/{ 1, 0, 1, 1, 1, 1, 1, 0, 1},
        /*9*/{ 0, 1, 0, 1, 1, 1, 0, 1, 0}
        };
    
    public List<Transform> AllLines = new List<Transform>();
    
    public Transform LineContainer;
    public Transform LineTemplate;
    public RectTransform[] dots;
    public RectTransform line;
    RectTransform startPoint;
    int startPointNum;
    public bool isDrawing;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = null;
        isDrawing = false;
        startPointNum = -1;
        line.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Clear_Current_Lines();
        }
        if (isDrawing && startPoint != null)
        {
            Draw_line(line, -1);
        }
    }

    void Draw_line(RectTransform DrawingLine, int FinishPoint)
    {
        
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(startPoint.parent as RectTransform, 
            Input.mousePosition, 
            null, // Using "null" since it's in Screen Space - Overlay; use "Camera.main" for World Space
            out mousePos);

        // Get the start position in UI space
        Vector2 startPos = new Vector2(startPoint.localPosition.x, startPoint.localPosition.y);
        Vector2 direction;
        // Calculate direction and distance
        if (FinishPoint == -1)
        {
           direction = mousePos - startPos; 
        }
        else
        {
            RectTransform FinishDot = dots[FinishPoint];
            Vector2 FinishPos = new Vector2(FinishDot.localPosition.x, FinishDot.localPosition.y);
            direction = FinishPos - startPos;
        }
        
        float distance = direction.magnitude;

        // Set position to midpoint
        DrawingLine.localPosition = startPos + direction * 0.5f;

        // Set rotation to look at the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        DrawingLine.localRotation = Quaternion.Euler(0, 0, angle);

        // Scale the line to match the distance
        DrawingLine.sizeDelta = new Vector2(distance, DrawingLine.sizeDelta.y);
    }

    public void Start_Line(int dot_num)
    {
        if(!check_row(dot_num))
        {
            startPoint = dots[dot_num];
            isDrawing = true;
            startPointNum = dot_num;
            line.gameObject.SetActive(true);
        }
    }

    bool check_row(int row)
    {
        for(int i = 0; i < 9; i++)
        {
            Debug.Log(CurrentSolution[row, i]);
            if(CurrentSolution[row, i] != ValidEdges[row, i])
                return false;
        }
        return true;
    }

    public void Clear_All_Lines()
    {
        while(AllLines.Count > 0)
        {
            Destroy(AllLines[0].gameObject);
            AllLines.RemoveAt(0);
        }

        for(int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                CurrentSolution[i,j] = 0;
            }
        }
    }
       

    public void Clear_Current_Lines()
    {
        if(isDrawing)
        {
            isDrawing = false;
            startPoint = null;
            startPointNum = -1;
            line.gameObject.SetActive(false);
        }
    }

    public void Finish_Line(int FinishPoint)
    {
        if(ValidEdges[startPointNum, FinishPoint] == 1 && CurrentSolution[startPointNum, FinishPoint] == 0)
        {
            Transform  newLine = Instantiate(LineTemplate, LineContainer);
            AllLines.Add(newLine);
            newLine.gameObject.SetActive(true);
            Draw_line(newLine.GetComponent<RectTransform>(), FinishPoint);
            CurrentSolution[startPointNum, FinishPoint] = 1;
            CurrentSolution[FinishPoint, startPointNum] = 1;
            isDrawing = false;
            startPoint = null;
            startPointNum = -1;
            line.gameObject.SetActive(false);

        }
    }


    
}


    
