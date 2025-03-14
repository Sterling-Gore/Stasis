using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject camera = GameObject.Find("Main Camera");
        GameObject op = GameObject.Find("OtherPoint");
        Vector3 pos = camera.transform.position;
        Vector3 camdir = (this.transform.position - camera.transform.position).normalized;
        Vector3 ptdir = (this.transform.position - op.transform.position).normalized;
        //Debug.Log(Vector3.Angle(camdir, ptdir));
    }
}
