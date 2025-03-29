using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.AI;
using SoundSource = PathNode;
using UnityEditor.Search;
using Unity.VisualScripting;

//THIS NEEDS A LOOOOOT OF CLEANUP
public class AlienController : Loadable
{
    public SoundSource curTarget;
    public SoundSource nextTarget;
    public GameObject player;

    public bool updateAudio = true;

    [Header("Decision Making")]
    public float attackRadius;
    public bool heardSomething = false;
    public float mentalDelay = 5.0f;
    public int soundSourcesMemory;

    [Header("Movement")]
    public float turnRadius;
    public float turnSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float tiredSpeed;
    [Header("Calculated Movement")]
    public float actualSpeed;
    public float timeStayingStill;
    public float curSpeed;
    public float nextSpeed;
    float timeInSpeed;

    [Header("Stamina")]
    public float restingPeriod;
    public float walkingStamina;
    public float runningStamina;

    Vector3 prevPos = new();

    Rigidbody playerRb;
    public PathGraph pathGraph;

    public PathFindingController pathFinder;
    RoamController roamer;
    public Transform head;
    public List<SoundSource> blackListedSoundSources = new();
    public GameObject soundSource;
    bool justHeardSomething;
    int curPowerLevel = -1;
    public List<Transform> curSections = new();
    Animator animator;

    AudioSource walkingAudio, idleAudio, attackAudio;
    AudioLowPassFilter walkingMufflerFilter;

    [Header("Audio")]
    public List<AudioClip> walkingClips = new();
    public List<AudioClip> idleClips = new(), attackClips = new();

    public float maxAudioDistance = 40;
    public float minAudioDistance = 0.5f;


    HealthSystem playerHealthSystem;
    [Header("Attack")]
    public float damageAmount = 0.1f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0;

    public static List<AlienController> aliens = new();

    static int ignoreAlienLayer, groundLayer;

    public bool isAwareOfPlayer = false;


    public enum State
    {
        Hunting,
        Roaming,
        Alert
    }

    public State currentState;

    NavMeshAgent NMA;
    [SerializeField]
    Transform previousNode;
    Vector3 endNodePosition;
    [SerializeField]
    Vector3[] nodePositions;

    public int CurrentAttention;

    bool isDormant;

    float angryTimer = 0;
    float pathPauseTimer = 0;

    Queue<Vector3> pathQueue;

    int attentionDecayPerSecond;

    [SerializeField]
    float roamingSpeed, huntingSpeed;

    IEnumerator attentionDecayFunc;
    [SerializeField] 
    float roamingAttentionTickRate, alertAttentionTickRate, huntingAttentionTickRate;
    [NonSerialized] 
    public float currentAttentionTickRate;

    float lookingAroundLength;

    [SerializeField]
    PowerDoors_Workshop bedroomDoor;

    void Start()
    {
        animator = GetComponent<Animator>();

        playerRb = player.GetComponent<Rigidbody>();

        curSpeed = nextSpeed = walkSpeed;

        //pathFinder = new(this);
        //roamer = GetComponent<RoamController>();

        ////UpdatePowerLevel(PowerLevel.instance.currentPowerLevel);
        ////PowerLevel.instance.SubscribeToUpdates(powerLevel => UpdatePowerLevel(powerLevel));

        //roamer.Init(this);

        Transform sounds = transform.Find("Sounds");
        idleAudio = sounds.Find("IdleSounds").gameObject.GetComponent<AudioSource>();
        walkingAudio = sounds.Find("WalkSounds").gameObject.GetComponent<AudioSource>();
        attackAudio = sounds.Find("AttackSounds").gameObject.GetComponent<AudioSource>();
        walkingMufflerFilter = sounds.Find("WalkSounds").gameObject.GetComponent<AudioLowPassFilter>();

        //aliens.Add(this);
        //ignoreAlienLayer = ~(
        //    1 << LayerMask.NameToLayer("AlienLayer")
        //);
        //groundLayer = 1 << LayerMask.NameToLayer("whatIsGround");

        playerHealthSystem = player.GetComponent<HealthSystem>();
        lastAttackTime = -attackCooldown;

        NMA = GetComponent<NavMeshAgent>();
        nodePositions = GetActivePathnodes();

        pathQueue = new Queue<Vector3>();
        currentState = State.Roaming;

        attentionDecayFunc = AttentionDecay();
        StartCoroutine(attentionDecayFunc);

        attentionDecayPerSecond = 3;
        currentAttentionTickRate = roamingAttentionTickRate;

        Vector3 newEndNode = nodePositions[Random.Range(0, nodePositions.Length - 1)];
        SetEndDestination(newEndNode);
        GoRoaming();

        isDormant = true;
        bedroomDoor.DoorActivated += BedroomDoorBreak;
    }

    void Update()
    {
        //animator.SetBool("isWalking", true);
        //animator.SetBool("isRunning", true);
        //animator.SetBool("isLookingAround", false);
        //return;
        //if (currentState == State.Roaming)
        //{
        //    Rigidbody rb = GetComponent<Rigidbody>();
        //    Vector3 v3Velocity = rb.velocity;

        //    if (rb.velocity.sqrMagnitude > 0 && !walkingAudio.isPlaying)
        //    {
        //        PlayRandomWalkAudio();
        //    }
        //}
        

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
            if (NMA.remainingDistance < NMA.stoppingDistance)
            {
                if (currentState != State.Alert)
                {
                    animator.SetBool("isLookingAround", true);
                    NMA.speed = huntingSpeed * Mathf.Clamp(Vector3.Distance(GetSoundPoopPosition(), transform.position) / 5, 1, 2);
                }
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
                angryTimer += Time.deltaTime;
            }
            if (angryTimer > 5)
            {
                CurrentAttention -= 30;
                angryTimer = 0;
                if (currentState == State.Hunting)
                {
                    WanderSoundPoop(3, 5);
                    GoRoaming();
                }
                else
                {
                    WanderSoundPoop(1, 7);
                    GoRoaming();
                }
            }

        }
        else if (pathQueue.Count == 0)
        {
            
            animator.SetBool("isWalking", false);
            if (pathPauseTimer < 5)
            {
                pathPauseTimer += Time.deltaTime;
                return;
            }

            pathPauseTimer = 0;

            Vector3 newEndNode = nodePositions[Random.Range(0, nodePositions.Length - 1)];

            while(Vector3.Distance(newEndNode, transform.position) < 2f)
                newEndNode = nodePositions[Random.Range(0, nodePositions.Length - 1)];

            animator.SetBool("isWalking", true);

            SetEndDestination(newEndNode);

        }
        else if (NMA.remainingDistance <= NMA.stoppingDistance)
        {
            pathQueue.Dequeue();
            if (pathQueue.Count != 0)
                NMA.SetDestination(pathQueue.Peek());
        }

        //if (!isAwareOfPlayer)
        //    return;

        //soundSource.transform.position = curTarget.pos;

        //animator.SetBool("isWalking", true);
        //animator.SetBool("isRunning", false);
        //if (!heardSomething)
        //{
        //    if (nextTarget != SoundSource.None)
        //    {
        //        curTarget = nextTarget;
        //        nextTarget = SoundSource.None;
        //        AnnounceHeardSomething();
        //    }
        //    nextSpeed = walkSpeed;
        //    roamer.RoamAround();
        //}
        //else
        //{
        //    if (Time.time - lastAttackTime >= attackCooldown)
        //        HuntPlayer();
        //    else
        //        RunFromPlayer();
        //}
        // Debug.DrawRay(transform.position + Vector3.up, player.transform.position - transform.position + Vector3.up);
    }

    //override protected void OnDestroy()
    //{
    //    aliens.Remove(this);
    //    pathFinder.Dispose();
    //    pathGraph.Dispose();
    //    base.OnDestroy();
    //}

    void KeepUpright()
    {
        transform.rotation.Set(0, 0, 0, 0);
    }

    void AnnounceHeardSomething()
    {
        PlayRandomIdleAudio();
        justHeardSomething = true;
        heardSomething = true;
        Debug.Log("I hear you");
    }

    void HuntPlayer()
    {
        if (
            nextTarget != SoundSource.None &&
            Vector3.Distance(nextTarget.pos, transform.position) < Vector3.Distance(curTarget.pos, transform.position)
        )
            curTarget = nextTarget;
        PlayRandomWalkAudio();
        var pos = transform.position;
        var playerPos = player.transform.position;
        playerPos.y = pos.y = (playerPos.y + pos.y + 1) / 2;

        var directionToPlayer = playerPos - pos;
        var distanceToPlayer = directionToPlayer.magnitude;

        Physics.Raycast(pos, directionToPlayer.normalized, out RaycastHit j, distanceToPlayer, ignoreAlienLayer);
        Debug.Log(j.rigidbody);
        Debug.DrawRay(pos, directionToPlayer, Color.green);
        if (j.rigidbody == playerRb)
        {
            // if (curSpeed == runSpeed)
            animator.SetBool("isRunning", true);
            nextSpeed = runSpeed;
            if (distanceToPlayer < attackRadius)
                AttackPlayer();
            else
                GoStraightToPlayer();
        }
        else if (!justHeardSomething && pathFinder.HasArrived())
        {
            blackListedSoundSources.Add(curTarget);
            if (blackListedSoundSources.Count > soundSourcesMemory)
                blackListedSoundSources.RemoveAt(0);

            roamer.FindCurrentRoom();
            heardSomething = false;
            if (nextTarget == SoundSource.None)
                Debug.Log("no longer heard anything");
        }
        else
        {
            nextSpeed = walkSpeed;
            pathFinder.CalculatePathPeriodically(curTarget.pos);
            pathFinder.FollowPath();
        }
        justHeardSomething = false;
    }

    void RunFromPlayer()
    {

    }

    void UpdatePowerLevel(int powerLevel)
    {
        if (powerLevel != curPowerLevel)
        {
            isAwareOfPlayer = powerLevel > 0;
            curSections = new(3);
            switch (powerLevel)
            {
                case 3:
                    goto case 2;
                case 2:
                    curSections.Add(GameObject.Find("SectionC").transform);
                    goto case 1;
                case 1:
                    curSections.Add(GameObject.Find("SectionB").transform);
                    goto case 0;
                case 0:
                    curSections.Add(GameObject.Find("SectionA").transform);
                    break;
            }

            //ReloadPathGraph();
            curPowerLevel = powerLevel;
            roamer.UpdateRooms(curSections);
        }
    }

    public void ReloadPathGraph()
    {
        pathGraph = new PathGraph(NodesInSections(curSections));
    }
    List<Transform> NodesInSections(List<Transform> pathNodeSections)
    {
        List<Transform> nodes = new();
        foreach (Transform section in pathNodeSections)
            foreach (Transform room in section)
                foreach (Transform pathNode in room)
                    nodes.Add(pathNode);
        return nodes;
    }

    void AttackPlayer()
    {
        Debug.Log("damage dealt player");
        PlayRandomAttackAudio();
        playerHealthSystem.TakeDamage(damageAmount, damageType.experiment87);
        lastAttackTime = Time.time;
        PlayRandomAttackAudio();
    }

    void GoStraightToPlayer()
    {
        MoveTowards(player.transform.position);
        //don't go back to path just recalculate path
        pathFinder.WillRecalculate();
    }

    // <returns>true if reached target</returns>
    public bool MoveTowards(Vector3 target)
    {
        CheckStayingStill();
        PlayRandomWalkAudio();
        target.y = transform.position.y;
        var targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

        var dPos = CurSpeed() * Time.deltaTime;

        prevPos = transform.position;
        transform.Translate(Vector3.forward * dPos);
        var curPos = transform.position;
        var closestPoint = GetClosestPointToLine(prevPos, (curPos - prevPos).normalized, prevPos - target);

        transform.position = Clamp(closestPoint, prevPos, curPos);

        CheckNewPosition();

        return Vector3.Distance(transform.position, target) <= dPos;
    }

    Vector3 GetClosestPointToLine(Vector3 origin, Vector3 direction, Vector3 point2origin) =>
        origin - Vector3.Dot(point2origin, direction) * direction;

    Vector3 Clamp(Vector3 point, Vector3 start, Vector3 end)
    {
        var start2end = (end - start).normalized;
        var start2point = (point - start).normalized;
        if (start2point != start2end)
            return start;
        var end2point = (point - start).normalized;
        if (end2point == start2end)
            return end;
        return point;
    }

    void CheckNewPosition()
    {
        var aboveNewPosition = transform.position;
        aboveNewPosition.y += 1;
        if (!Physics.Raycast(aboveNewPosition, Vector3.down, 10, groundLayer))
        {
            Debug.DrawRay(aboveNewPosition, Vector3.down * 10, Color.black, 20);
            Debug.Log($"point not above ground {aboveNewPosition}"); // Change to just Log instead of Error as these can occur depending on object geo. 
            transform.position = prevPos;
        }
    }

    void CheckStayingStill()
    {
        actualSpeed = Vector3.Distance(prevPos, transform.position) / Time.deltaTime;
        if (actualSpeed < .5)
        {
            timeStayingStill += Time.deltaTime;
            if (timeStayingStill > 2)
            {
                Debug.Log($"alien is stuck! was hunting: {heardSomething}, current state: {roamer.state}"); // Change to log, Same issue here
                animator.SetBool("isWalking", false);
                roamer.FindCurrentRoom();
                if (!heardSomething)
                    roamer.curState.OnStuck();
                else
                    heardSomething = false;
                timeStayingStill = 0;
            }
        }
        else
            timeStayingStill = 0;
    }

    float CurSpeed()
    {
        timeInSpeed += Time.deltaTime;
        if (curSpeed == runSpeed && timeInSpeed > runningStamina)
        {
            curSpeed = walkSpeed;
            timeInSpeed = 0;
        }
        else if (curSpeed == walkSpeed && timeInSpeed > walkingStamina)
        {
            curSpeed = tiredSpeed;
            timeInSpeed = 0;
        }
        else if (curSpeed == tiredSpeed && timeInSpeed > restingPeriod)
        {
            curSpeed = nextSpeed;
            timeInSpeed = 0;
        }

        return curSpeed;
    }

    public void PlayRandomWalkAudio()
    {
        RaycastHit[] hits;
        LayerMask wallMask = LayerMask.GetMask("Surfaces");

        hits = Physics.RaycastAll(player.transform.position,
            (transform.position - player.transform.position).normalized,
            Vector3.Distance(player.transform.position, transform.position),
            wallMask);

        int initialThreshold = 5000;
        float initialVolume = 1;
        if(updateAudio)
            walkingAudio.volume = hits.Length > 0 ? (initialVolume* (0.5f / hits.Length)) : initialVolume;
        walkingMufflerFilter.cutoffFrequency = hits.Length > 0 ? (int)(initialThreshold * (0.6f / hits.Length)) : initialThreshold;
        PlayRandomAudio(walkingAudio, walkingClips);
    }

    public void PlayRandomAttackAudio() => PlayRandomAudio(attackAudio, attackClips);

    public void PlayRandomIdleAudio() => PlayRandomAudio(idleAudio, idleClips);

    void PlayRandomAudio(AudioSource audioSource, List<AudioClip> audioClips)
    {
        if (Time.timeScale > 0 && audioClips.Count != 0)
        {

            //float distance = Vector3.Distance(transform.position, player.transform.position);
            //if (distance > maxAudioDistance)
            //    audioSource.volume = 0;
            //else
            //{
            //    float volume = Mathf.Lerp(1, 0, (distance - minAudioDistance) / (maxAudioDistance - minAudioDistance));
            //    audioSource.volume = volume;
            //}

            //play one shot does not work for some reason even though all the documentation points to it should being able to work >:c
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);


        }
    }

    public override void Load(JObject state)
    {
        LoadTransform(state);
        heardSomething = false;
    }

    public override void Save(ref JObject state) =>
        SaveTransform(ref state);


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
    void SetEndDestination(Vector3 endDestination)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        NMA.CalculatePath(endDestination, navMeshPath);

        Vector3[] corners = navMeshPath.corners;
        if (corners.Length == 0)
            Debug.Log("No corners found on path");

        pathQueue = CalculatePathQueue(corners);
        NMA.SetDestination(pathQueue.Peek());

        //foreach (Vector3 corner in corners)
        //{
        //    Debug.DrawLine(corner, corner + Vector3.up * 100, Color.blue, Mathf.Infinity);
        //}
    }

    Queue<Vector3> CalculatePathQueue(Vector3[] corners)
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
            .Where(node => node.activeInHierarchy)
            .Select(node => node.transform.position)
            .ToArray();

    public void UpdatePathNodes() => nodePositions = GetActivePathnodes();

    //-------------------------------------------------------------------------------------------

    //attention functions
    IEnumerator AttentionDecay()
    {
        while (true)
        {
            if(CurrentAttention > attentionDecayPerSecond)
                CurrentAttention = CurrentAttention - attentionDecayPerSecond;
            //CurrentAttention = Mathf.Clamp(CurrentAttention - attentionDecayPerSecond, 0, 100);
            yield return new WaitForSeconds(1f);
        }
    }
    //Make a gameObject at the last known "sound"
    GameObject GenerateSoundPoop(Vector3 attentionLocation)
    {
        GameObject otherPoop = GameObject.Find("Sound Poop");
        if (otherPoop != null) Destroy(otherPoop);

        GameObject soundPoop = new GameObject("Sound Poop");

        soundPoop.transform.position = attentionLocation;
        return soundPoop;
    }

    Vector3 GetSoundPoopPosition() => GameObject.Find("Sound Poop").transform.position;

    public void IncreaseAttention(int attention, Vector3 attentionLocation)
    {
        if (isDormant) return;

        CurrentAttention = Mathf.Clamp(CurrentAttention + attention, 0, 100);

        //ranges 41-1 as attention gets higher
        int alertAttentionThreshold = (int) (40-0.5f * CurrentAttention);

        //Debug.Log("Current attention: " + CurrentAttention);

        if (CurrentAttention == 100 && attention > 10)
            GoHunting(attentionLocation);
        else if (currentState != State.Hunting && attention > alertAttentionThreshold && attention > 1)
            GoAlert(attentionLocation);
    }

    void GoHunting(Vector3 attentionLocation)
    {
        animator.SetBool("isRunning", true);
        if (currentState != State.Hunting)
        {
            NMA.speed = huntingSpeed * Mathf.Clamp(Vector3.Distance(attentionLocation, transform.position)/5, 1.5f, 2);
            StartCoroutine(RepeatAttackingSound());
            //inInitialCharge = true;
            //InitialCharge(attentionLocation);
            StartCoroutine(SpeedDecay());
        }

        angryTimer = 0f;
        currentState = State.Hunting;

        GameObject soundPoop = GenerateSoundPoop(attentionLocation);
        
        //NMA.velocity = (attentionLocation - transform.position).normalized * 100;
        //NMA.speed = huntingSpeed;
        currentAttentionTickRate = huntingAttentionTickRate;
        NMA.angularSpeed = 360;
        NMA.SetDestination(soundPoop.transform.position);
    }

    IEnumerator SpeedDecay()
    {
        bool flag = false;
        while (currentState == State.Hunting || !flag)
        {
            if (currentState == State.Hunting)
                flag = true;
            NMA.speed = Mathf.Clamp(NMA.speed - 0.5f, huntingSpeed-1, huntingSpeed * 2);
            yield return new WaitForSeconds(1f);
        }
    }

    void InitialCharge(Vector3 attentionLocation)
    {
        StopCoroutine(attentionDecayFunc);
        NMA.ResetPath();

        GameObject soundPoop = GenerateSoundPoop(attentionLocation);

        //NMA.velocity = (attentionLocation - transform.position).normalized * 100;
        NMA.speed = huntingSpeed*3;
        NMA.angularSpeed = 360;
        NMA.SetDestination(soundPoop.transform.position);
    }

    IEnumerator RepeatAttackingSound()
    {
        bool flag = false;
        while(currentState == State.Hunting || !flag)
        {
            if(currentState == State.Hunting)
                flag = true;
            PlayRandomAttackAudio();
            yield return new WaitForSeconds(5f);
        }
    }

    

    void GoAlert(Vector3 attentionLocation)
    {
        PlayRandomIdleAudio();
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isLookingAround", false);

        angryTimer = 0f;
        StopCoroutine(attentionDecayFunc);
        currentAttentionTickRate = alertAttentionTickRate;
        GameObject soundPoop = GenerateSoundPoop(attentionLocation);
        currentState = State.Alert;
        NMA.ResetPath();
        transform.LookAt(soundPoop.transform.position);
    }

    void WanderSoundPoop(int times, float range)
    {
        NMA.speed = roamingSpeed;
        //i dont really wanna do this
        GameObject soundPoop = GameObject.Find("Sound Poop");
        Vector3 position = soundPoop.transform.position;

        pathQueue.Clear();

        int iterationLimit = 100;
        int iterations = 0;

        while (pathQueue.Count < times && iterations < iterationLimit)
        {
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

    void GoRoaming()
    {
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isLookingAround", true);
        StartCoroutine(attentionDecayFunc);
        NMA.speed = roamingSpeed;
        NMA.angularSpeed = 120;
        currentAttentionTickRate = roamingAttentionTickRate;
        currentState = State.Roaming;
        NMA.SetDestination(pathQueue.Peek());
    }

    void BedroomDoorBreak(object s, EventArgs e)
    {
        GoHunting(GameObject.Find("M-9").transform.position);
        isDormant = false;
    }
}




/*
 * Ideas:
 * To save on CPU usage, only run A* every now and then
 * Path will be calculated and alien will follow it until its close enough to target
 * Alien will use path nodes defined under the "AlienPathNodes" gameobject. 
 * 
 * Also if theres a ray from alien to target with nothing in between go straight
 * 
 * 
 * TODO: make attack, make path finding for noise/specific events
 cases:
 pathing to room -> pathing to player
 pathing in room -> pathing to player
 didn't reach room 
 pathing to player-> pathing to room
 
 */

