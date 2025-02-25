using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlueDot : MonoBehaviour
{
    public Transform originPos;
    public Transform playerPos;
    Vector3 relativePos;
    public RectTransform dot;
    public RectTransform UI_Origin;
    public float mapScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        relativePos = playerPos.position - originPos.position;
        //Debug.Log(relativePos);
        //dot.position = new Vector3(destinationVector.x +440, destinationVector.z + 290, dot.position.z );
        //dot.position = dot.position + UI_Origin.position;


        Vector2 uiPosition = new Vector2(relativePos.x, relativePos.z) * mapScale;

        // Set the dot's anchored position relative to the UI origin
        dot.anchoredPosition = UI_Origin.anchoredPosition + uiPosition;
    }
}
