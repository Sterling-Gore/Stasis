using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public void DeactivateAllEyes() => transform.Cast<Transform>()
        .ToList().ForEach(child => child.gameObject.SetActive(false));

    public void RandomlyActivateEyes(float percent) => transform.Cast<Transform>()
        .Where(child => Random.Range(0f, 1f) < percent)
        .ToList().ForEach(child => child.gameObject.SetActive(true));
}
