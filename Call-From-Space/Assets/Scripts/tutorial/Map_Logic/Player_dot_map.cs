using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_dot_map : MonoBehaviour
{
    [SerializeField] public Transform Game_Origin_Pos; //(0,0)
    [SerializeField] public Transform Game_End_Pos; //(1,1)
    [SerializeField] public Transform playerPos;
    Vector3 relativePos;
    [SerializeField] public RectTransform dot;
    [SerializeField] public RectTransform UI_Origin_Pos;
    [SerializeField] public RectTransform UI_End_Pos;
    public bool vertical = false;

    float x_percentage;
    float y_percentage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(vertical)
        {
            y_percentage = Mathf.Abs((playerPos.position.x - Game_Origin_Pos.position.x) / (Game_End_Pos.position.x - Game_Origin_Pos.position.x));
            x_percentage = Mathf.Abs((playerPos.position.z - Game_Origin_Pos.position.z) / (Game_End_Pos.position.z - Game_Origin_Pos.position.z));
        }
        else
        {
            x_percentage = Mathf.Abs((playerPos.position.x - Game_Origin_Pos.position.x) / (Game_End_Pos.position.x - Game_Origin_Pos.position.x));
            y_percentage = Mathf.Abs((playerPos.position.z - Game_Origin_Pos.position.z) / (Game_End_Pos.position.z - Game_Origin_Pos.position.z));
        }
        float xValue = Mathf.Abs((UI_End_Pos.anchoredPosition.x - UI_Origin_Pos.anchoredPosition.x) * x_percentage);
        float yValue = Mathf.Abs((UI_End_Pos.anchoredPosition.y - UI_Origin_Pos.anchoredPosition.y) * y_percentage);
        //relativePos = playerPos.position - originPos.position;
        //Debug.Log(relativePos);
        //dot.position = new Vector3(destinationVector.x +440, destinationVector.z + 290, dot.position.z );
        //dot.position = dot.position + UI_Origin.position;


        Vector2 uiPosition = new Vector2(xValue, yValue);

        // Set the dot's anchored position relative to the UI origin
        dot.anchoredPosition = UI_Origin_Pos.anchoredPosition + uiPosition;
    }
}
