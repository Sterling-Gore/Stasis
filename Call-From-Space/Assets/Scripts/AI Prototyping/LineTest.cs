using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            Debug.DrawLine(child.position,child.position + Vector3.up * 100, Color.red, Mathf.Infinity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
