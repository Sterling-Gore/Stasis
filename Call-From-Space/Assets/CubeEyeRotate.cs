using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEyeRotate : MonoBehaviour
{

    static Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, Vector3.left);
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            child.LookAt(player.position);
            child.rotation = child.rotation * rotation;
        }
    }
}
