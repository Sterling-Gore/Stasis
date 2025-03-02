using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    public bool active = true;

    // Update is called once per frame
    void Update()
    {
        if(active)
            transform.position = cameraPosition.position;
    }
}
