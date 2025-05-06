using System;
using System.Collections;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathnodeManager : MonoBehaviour
{
    Dictionary<PowerDoors_Workshop, GameObject> doorNodesDict;

    public PowerDoors_Workshop[] eventDoors;

    Transform playerTransform;
    AlienController alienController;
    Vector3[] activeNodes;

    [SerializeField]
    int defaultGoToPlayerChance, goToPlayerChanceIncrement;
    int currentGoToPlayerChance;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        alienController = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienController>();
        foreach (PowerDoors_Workshop door in eventDoors)
        {
            door.DoorActivated += ActivateNodes;
        }
        currentGoToPlayerChance = defaultGoToPlayerChance;
        UpdatePathNodes();
    }

    void ActivateNodes(object sender, DoorEventArgs e)
    {
        Debug.Log("Subscriber called");
        GameObject[] nodes = e.associatedNodes;
        foreach (GameObject node in nodes)
        {
            node.SetActive(true);
        }
       UpdatePathNodes();
    }

    public Queue<Vector3> CalculatePathQueue(Vector3[] corners)
    {
        Queue<Vector3> pathQueue = new Queue<Vector3>();

        foreach (Vector3 corner in corners)
        {
            Vector3 closestNode = findNearestNode(corner);
            if (!pathQueue.Contains(closestNode))
                pathQueue.Enqueue(closestNode);
        }

        return pathQueue;
    }

    public Vector3 findNearestNode(Vector3 position, bool mustBeVisible = false)
    {

        Vector3 closest = Vector3.one * Mathf.Infinity;
        Vector3 positionToClosest = closest - position;

        foreach (Vector3 nodePosition in activeNodes)
        {
            Vector3 positionToNode = nodePosition - position;

            //REFACTOR THESE CONDITIONALS, I HATE THEM this might not even be necessary
            if (positionToClosest.sqrMagnitude > positionToNode.sqrMagnitude)
            {
                if ((mustBeVisible && Physics.Raycast(transform.position, positionToNode.normalized, positionToNode.magnitude)) || !mustBeVisible)
                {
                    closest = nodePosition;
                    positionToClosest = positionToNode;
                }
            }
        }

        return closest;
    }

    Vector3[] GetActivePathnodes() => GameObject.FindGameObjectsWithTag("Pathnode")
            .Where(node => node.activeInHierarchy) //conditional
            .Select(node => node.transform.position) //transform
            .ToArray();

    public void UpdatePathNodes() => activeNodes = GetActivePathnodes();

    public Vector3 GetRandomNode()
    {
        int randomNum = Random.Range(0, 100);

        if (randomNum < currentGoToPlayerChance)
        {
            Debug.Log("Went to player");
            currentGoToPlayerChance = defaultGoToPlayerChance;
            return findNearestNode(playerTransform.position);
        }
        else
        {
            currentGoToPlayerChance += goToPlayerChanceIncrement;
            return activeNodes[Random.Range(0, activeNodes.Length - 1)];
        }
    }

}
