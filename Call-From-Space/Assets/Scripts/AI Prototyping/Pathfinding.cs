using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;



public class Pathfinding : MonoBehaviour
{

    enum State
    {
        Hunting,
        Roaming,
        Alert
    }

    [SerializeField]
    State currentState;

    public bool withoutModification;

    NavMeshAgent NMA;
    [SerializeField]
    Transform previousNode;
    Vector3 endNodePosition;
    [SerializeField]
    Vector3[] nodePositions;

    public int CurrentAttention;

    bool isTrackingSound;
    float angryTimer = 0;
    float pathPauseTimer = 0;

    Queue<Vector3> pathQueue;

    int attentionDecayPerSecond;

    readonly int roamingSpeed = 20;
    readonly int chasingSpeed = 60;

    IEnumerator attentionDecayFunc;

    // Start is called before the first frame update
    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        nodePositions = GameObject.FindGameObjectsWithTag("PathNode").Select(node => node.transform.position).ToArray();
        endNodePosition = GameObject.Find("End").transform.position;
        pathQueue = new Queue<Vector3>();

        currentState = State.Roaming;

        if (withoutModification) NMA.SetDestination(endNodePosition);

        attentionDecayFunc = AttentionDecay();
        StartCoroutine(attentionDecayFunc);

        attentionDecayPerSecond = 1;
    }
    // Update is called once per frame
    void Update()
    {

        if (withoutModification) return;

        if (currentState == State.Hunting || currentState == State.Alert)
        {

            if (currentState == State.Hunting && NMA.remainingDistance <= NMA.stoppingDistance)
            {
                NMA.ResetPath();
                //Vector3 nearest = findNearestNode(transform.position, true);
                //setEndDestination(nearest);
            }

            //NavMeshHit hit;
            //if(NavMesh.FindClosestEdge(transform.position, out hit, NavMesh.AllAreas) && hit.distance < 0.01f)
            //{
            //   // Debug.Log("edge reached");

            //    //me trying to full stop the agent when it hits a wall, really not working properly
            //    NMA.destination = transform.position;
            //    float tempAcc = NMA.acceleration;
            //    float tempSpeed = NMA.speed;
            //    NMA.angularSpeed = 0;
            //    NMA.acceleration = 1000000;
            //    NMA.speed = 0;
            //    NMA.stoppingDistance = 0;
            //    StartCoroutine(ResetValues(tempAcc, tempSpeed));
            //}

            if (currentState == State.Hunting)
            {
                
                angryTimer += Time.deltaTime;
                if (angryTimer > 5)
                {
                    angryTimer = 0;
                    wanderSoundPoop(5, 50);
                    goRoaming();
                }
            }

        }
        else if (pathQueue.Count == 0)
        {
            pathPauseTimer += Time.deltaTime;
            if (pathPauseTimer < 2) return;
            pathPauseTimer = 0;

            Vector3 newEndNode = nodePositions[Random.Range(0,nodePositions.Length-1)];
            setEndDestination(newEndNode);
            
        }
        else if (NMA.remainingDistance <= NMA.stoppingDistance)
        {
            pathQueue.Dequeue();
            if(pathQueue.Count != 0) 
                NMA.SetDestination(pathQueue.Peek());
        }
    }

    //IEnumerator ResetValues(float tempAcc, float tempSpeed)
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    NMA.speed = roamingSpeed;
    //    NMA.acceleration = 50;
    //    NMA.angularSpeed = 120;
    //    NMA.stoppingDistance = 10;
    //    //NMA.ResetPath();
    //}

    //pathfinding functions
    void setEndDestination(Vector3 endDestination)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        NMA.CalculatePath(endDestination, navMeshPath);

        Vector3[] corners = navMeshPath.corners;
        pathQueue = calculatePathQueue(corners);

        NMA.SetDestination(pathQueue.Peek());

        //foreach (Vector3 corner in corners)
        //{
        //    Debug.DrawLine(corner, corner + Vector3.up * 100, Color.blue, Mathf.Infinity);
        //}
    }

    Queue<Vector3> calculatePathQueue(Vector3[] corners)
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

    Vector3 findNearestNode(Vector3 position, bool mustBeVisible = false) 
    {
        
        Vector3 closest = Vector3.one * Mathf.Infinity;
        Vector3 positionToClosest = closest - position;

        foreach (Vector3 nodePosition in nodePositions)
        {
            Vector3 positionToNode = nodePosition - position;

            //REFACTOR THESE CONDITIONALS, I HATE THEM this might not even be necessary
            if(positionToClosest.sqrMagnitude > positionToNode.sqrMagnitude)
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


    //attention functions
    IEnumerator AttentionDecay()
    {
        while (true)
        {
            CurrentAttention = Mathf.Clamp(CurrentAttention - attentionDecayPerSecond, 0, 100);
            yield return new WaitForSeconds(1f);
        }
    }
    //Make a gameObject at the last known "sound"
    GameObject generateSoundPoop(Vector3 attentionLocation)
    {
        GameObject otherPoop = GameObject.Find("Sound Poop");
        if (otherPoop != null) Destroy(otherPoop);

        GameObject soundPoop = new GameObject("Sound Poop");

        soundPoop.transform.position = attentionLocation;
        return soundPoop;
    }

    public void IncreaseAttention(int attention, Vector3 attentionLocation)
    {
        //if (currentState == State.Hunting) return;

        CurrentAttention = Mathf.Clamp(CurrentAttention + attention, 0, 100);

        //ranges 10-60 approaches 10 as attention gets higher
        int alertAttentionThreshold = 61 - (CurrentAttention / 2 + 1);

        Debug.Log("Current attention: " + CurrentAttention);

        if (CurrentAttention == 100 && attention > 10)
            goHunting(attentionLocation);
        else if (attention > alertAttentionThreshold || (CurrentAttention > 80 && attention > 0))
            goAlert(attentionLocation);
    }

    void goHunting(Vector3 attentionLocation) 
    {
        Debug.Log("Hi");
        angryTimer = 0f;
        StopCoroutine(attentionDecayFunc);
        NMA.ResetPath();

        GameObject soundPoop = generateSoundPoop(attentionLocation);
        currentState = State.Hunting;
        //NMA.velocity = (attentionLocation - transform.position).normalized * 100;
        NMA.speed = chasingSpeed;
        NMA.SetDestination(soundPoop.transform.position);
    }

    void goAlert(Vector3 attentionLocation)
    {
        StopCoroutine (attentionDecayFunc);
        GameObject soundPoop = generateSoundPoop(attentionLocation);
        currentState = State.Alert;
        NMA.ResetPath();
        transform.LookAt(soundPoop.transform.position);
    }

    void wanderSoundPoop(int times, float range)
    {
        Debug.Log("Called");
        NMA.speed = roamingSpeed;
        //i dont really wanna do this
        GameObject soundPoop = GameObject.Find("Sound Poop");
        Vector3 position = soundPoop.transform.position;

        pathQueue.Clear();

        int iterationLimit = 100;
        int iterations = 0;

        while (pathQueue.Count < times && iterations < iterationLimit) {
            iterations++;
            Vector3 randomCirclePointXY = Random.insideUnitCircle;
            Vector3 randomCirclePointXZ = new Vector3(randomCirclePointXY.x, 0f, randomCirclePointXY.y);
            Vector3 randomPoint = position + randomCirclePointXZ * range;
            Debug.Log(randomPoint);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                pathQueue.Enqueue(hit.position);
                Debug.DrawLine(randomPoint, randomPoint + Vector3.up * 100, Color.yellow, Mathf.Infinity);
            }
        }
    }

    void goRoaming()
    {
        StartCoroutine(attentionDecayFunc);
        NMA.speed = roamingSpeed;
        currentState = State.Roaming;
        NMA.SetDestination(pathQueue.Peek());
    }
}
