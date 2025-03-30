using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathnodeManager : MonoBehaviour
{
    Dictionary<PowerDoors_Workshop, GameObject> doorNodesDict;

    public PowerDoors_Workshop[] eventDoors;
    // Start is called before the first frame update
    AlienController alienController;
    void Awake()
    {
        alienController = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienController>();
        foreach (PowerDoors_Workshop door in eventDoors)
        {
            door.DoorActivated += ActivateNodes;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ActivateNodes(object sender, DoorEventArgs e)
    {
        Debug.Log("Subscriber called");
        GameObject[] nodes = e.associatedNodes;
        foreach (GameObject node in nodes)
        {
            node.SetActive(true);
        }
        alienController.UpdatePathNodes();
    }
}
