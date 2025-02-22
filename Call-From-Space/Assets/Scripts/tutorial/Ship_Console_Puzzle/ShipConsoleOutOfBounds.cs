using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShipConsoleOutOfBounds : MonoBehaviour, IPointerEnterHandler
{
    public Dot_Grid GridLeft;
    public Dot_Grid GridMiddle;
    public Dot_Grid GridRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("YO WTF");
        GridLeft.Clear_Current_Lines();
        GridMiddle.Clear_Current_Lines();
        GridRight.Clear_Current_Lines();
    }
}
