using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.AI;
using SoundSource = PathNode;
using Random = UnityEngine.Random;
//using UnityEditor.Search;
using Unity.VisualScripting;

//THIS NEEDS A LOOOOOT OF CLEANUP
public class AlienController : Loadable
{
    public enum State
    {
        Hunting,
        Roaming,
        Alert
    }

    public GameObject player;
    Rigidbody playerRb;
    public Transform head;
    Animator animator;


    [Header("---Audio---"), Space(10)]
    public List<AudioClip> walkingClips = new();
    public List<AudioClip> idleClips = new(), attackClips = new();
    public bool updateAudio = true;

    AudioSource walkingAudio, idleAudio, attackAudio;
    AudioLowPassFilter walkingMufflerFilter;

    [Header("---Movement---"), Space(10)]
    [SerializeField]
    float roamingSpeed;
    [SerializeField]
    float huntingSpeed;

    NavMeshAgent NMA;
    Queue<Vector3> pathQueue;

    [Header("---Attention---"), Space(10)]
    public State currentState;
    public int CurrentAttention;
    IEnumerator attentionDecayFunc;
    [SerializeField]
    float roamingAttentionTickRate, alertAttentionTickRate, huntingAttentionTickRate, attentionDecayPerSecond;
    [NonSerialized]
    public float currentAttentionTickRate;

    bool isDormant;
    public bool isPaused { get; private set; }
    float angryTimer = 0;
    float pathPauseTimer = 0;
    //maybe put this to use
    float lookingAroundLength;


    [Header("---Misc---"), Space(10)]
    [SerializeField]
    PowerDoors_Workshop bedroomDoor;
    [SerializeField]
    GameObject specialLockerNode;
    PathnodeManager nodeManager;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        nodeManager = GameObject.Find("AlienPathNodes").GetComponent<PathnodeManager>();
        playerRb = player.GetComponent<Rigidbody>();

        Transform sounds = transform.Find("Sounds");
        idleAudio = sounds.Find("IdleSounds").gameObject.GetComponent<AudioSource>();
        walkingAudio = sounds.Find("WalkSounds").gameObject.GetComponent<AudioSource>();
        attackAudio = sounds.Find("AttackSounds").gameObject.GetComponent<AudioSource>();
        walkingMufflerFilter = sounds.Find("WalkSounds").gameObject.GetComponent<AudioLowPassFilter>();
        NMA = GetComponent<NavMeshAgent>();
        isDormant = true;
        currentAttentionTickRate = roamingAttentionTickRate;
        pathQueue = new Queue<Vector3>();
        currentState = State.Roaming;
        attentionDecayFunc = AttentionDecay();
    }

    void Start()
    {

        StartCoroutine(attentionDecayFunc);

        SetEndDestination(nodeManager.GetRandomNode());
        GoRoaming();

        bedroomDoor.DoorActivated += BedroomDoorBreak;
    }

    void Update()
    {
        if (currentState == State.Hunting || currentState == State.Alert)
        {
            if (Vector3.Distance(NMA.destination, transform.position) < NMA.stoppingDistance)
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
                Debug.Log("Timer passed");
                CurrentAttention -= 30;
                angryTimer = 0;
                if (currentState == State.Hunting)
                {
                    WanderSoundPoop(1, 3);
                    GoRoaming();
                }
                else
                {
                    WanderSoundPoop(1, 1);
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

            Vector3 newEndNode = nodeManager.GetRandomNode();

            while (Vector3.Distance(newEndNode, transform.position) < 2f)
                newEndNode = nodeManager.GetRandomNode();

            animator.SetBool("isWalking", true);

            SetEndDestination(newEndNode);

        }
        else if (NMA.remainingDistance <= NMA.stoppingDistance)
        {
            pathQueue.Dequeue();
            if (pathQueue.Count != 0)
                NMA.SetDestination(pathQueue.Peek());
        }
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
        if (updateAudio)
            walkingAudio.volume = hits.Length > 0 ? (initialVolume * (0.5f / hits.Length)) : initialVolume;
        walkingMufflerFilter.cutoffFrequency = hits.Length > 0 ? (int)(initialThreshold * (0.6f / hits.Length)) : initialThreshold;
        PlayRandomAudio(walkingAudio, walkingClips);
    }

    public void PlayRandomAttackAudio() => PlayRandomAudio(attackAudio, attackClips);

    public void PlayRandomIdleAudio() => PlayRandomAudio(idleAudio, idleClips);

    void PlayRandomAudio(AudioSource audioSource, List<AudioClip> audioClips)
    {
        if (Time.timeScale > 0 && audioClips.Count != 0)
        {
            audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
        }
    }

    public override void Load(JObject state)
    {
    }

    public override void Save(ref JObject state) =>
        SaveTransform(ref state);

    //pathfinding functions
    void SetEndDestination(Vector3 endDestination)
    {
        NavMeshPath navMeshPath = new NavMeshPath();
        NMA.CalculatePath(endDestination, navMeshPath);

        Vector3[] corners = navMeshPath.corners;
        if (corners.Length == 0)
            Debug.Log("No corners found on path");

        pathQueue = nodeManager.CalculatePathQueue(corners);
        NMA.SetDestination(pathQueue.Peek());

        //foreach (Vector3 corner in corners)
        //{
        //    Debug.DrawLine(corner, corner + Vector3.up * 100, Color.blue, Mathf.Infinity);
        //}
    }

    

    //-------------------------------------------------------------------------------------------

    //attention functions
    IEnumerator AttentionDecay()
    {
        while (true)
        {
            if (CurrentAttention > attentionDecayPerSecond)
                CurrentAttention = CurrentAttention - (int)attentionDecayPerSecond;
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
        int alertAttentionThreshold = (int)(40 - 0.5f * CurrentAttention);

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
            NMA.speed = huntingSpeed * Mathf.Clamp(Vector3.Distance(attentionLocation, transform.position) / 5, 1.5f, 2);
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
        float timer = 15f;

        bool flag = false;
        while (currentState == State.Hunting || !flag)
        {
            Debug.Log(timer);
            if (currentState == State.Hunting)
                flag = true;
            NMA.speed = Mathf.Clamp(NMA.speed - 0.5f, huntingSpeed - 1, huntingSpeed * 2);

            if (timer <= 0)
            {
                NMA.speed = huntingSpeed * Mathf.Clamp(Vector3.Distance(GetSoundPoopPosition(), transform.position) / 5, 1.5f, 2);
                timer = 15f;
            }

            timer -= 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator RepeatAttackingSound()
    {
        bool flag = false;
        while (currentState == State.Hunting || !flag)
        {
            if (currentState == State.Hunting)
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
        isDormant = false;
        PowerDoors_Workshop genbdoor = GameObject.Find("(special)BackHallwayToGenBDoor").GetComponentInChildren<PowerDoors_Workshop>();

        if (genbdoor.poweredOn) return;
        GoHunting(GameObject.Find("M-9").transform.position);

    }

    public void ToggleAlien()
    {
        if (isPaused)
        {
            GoRoaming();
            isDormant = false;
            this.enabled = true;
            isPaused = false;
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isLookingAround", false);

            NMA.ResetPath();
            isDormant = true;
            this.enabled = false;
            isPaused = true;
        }
    }


    public void LockerRoomSequence()
    {
        if (currentState == State.Hunting) return;

        specialLockerNode.SetActive(true);
        nodeManager.UpdatePathNodes();

        CurrentAttention = 0;
        GoRoaming();
        NMA.Warp(GameObject.Find("H-0").transform.position);
        //if(!isPaused)
        //    ToggleAlien();


        SetEndDestination(specialLockerNode.transform.position);
    }
}

