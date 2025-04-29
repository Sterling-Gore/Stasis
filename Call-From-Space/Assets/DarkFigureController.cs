using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Device;
using UnityEngine.UIElements;
using Application = UnityEngine.Application;
using Random = UnityEngine.Random;
using Screen = UnityEngine.Screen;

public class DarkFigureController : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField]
    float maxJumpRangeFromPlayer;
    public bool isHarmless;

    [Header("Position Tracking")]
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform darkFigureHead, LOSPointsTransform, timeOutSquare;

    [Header("Hunting")]
    [SerializeField]
    Transform teleportAround;
    [SerializeField] 
    float minimumPlayerTeleportDistance;

    [Header("Jumpscare")]
    [SerializeField]
    AudioClip jumpscareNoise;
    [SerializeField]
    GameObject scareImage;

    ShadowRealm shadowRealmController;
    LOSChecker los;
    NavMeshAgent NMA;
    Collider caughtCollider;
    bool hunting, inTimeout;
    LayerMask surfacesMask;
    AudioSource scaryNoiseSource, jumpscareNoiseSource;
    Transform[] losPoints;
    

    Vector3 currentUp;

    private void Awake()
    {
        FindObjectOfType<SaveManager>().SaveRecieved += Initialize;
    }

    // Start is called before the first frame update
    void Start()
    {
        los = GetComponentInChildren<LOSChecker>();
        NMA = GetComponent<NavMeshAgent>();
        InsanityMeter.Instance.MaxInsanity += StartHunting;
        shadowRealmController = FindObjectOfType<ShadowRealm>();
        caughtCollider = GetComponent<Collider>();
        caughtCollider.enabled = false;
        hunting = false;
        surfacesMask = LayerMask.GetMask("Surfaces");
        currentUp = Vector3.up;
        
        losPoints = LOSPointsTransform.Cast<Transform>().ToArray();

        scaryNoiseSource = teleportAround.GetComponentInChildren<AudioSource>();
        jumpscareNoiseSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0 || !InsanityMeter.Instance.acceptingInsanityIncrease || inTimeout) return;

        if (losPoints.All(point => !los.isOnScreen(point.position)))
        {
            int rng = Random.Range(1, 200);
            int iterationLimit = 1000;
            int iterations = 0;
            if (rng == 1)
            {

                while (!TryJumpRandomSurface() && iterations++ < iterationLimit);
                if (iterations >= iterationLimit-1) 
                {
                    Debug.LogWarning("Iteration limit reached for TryJumpRandomSurface");
                    SendToTimeout(10f);
                }
                
            }
        }
    }

    private void LateUpdate()
    {
        darkFigureHead.LookAt(player.position + Vector3.up * 2);
    }

    private void FixedUpdate()
    {
        if (hunting)
        {
            transform.LookAt(player.position);
            float range = 1f;
            Vector3 randomCirclePointXY = Random.insideUnitCircle;
            Vector3 randomCirclePointXZ = new Vector3(randomCirclePointXY.x, 0f, randomCirclePointXY.y);
            Vector3 randomPoint = teleportAround.position + randomCirclePointXZ * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
            {
                NMA.Warp(hit.position);
                //Debug.DrawLine(randomPoint, randomPoint + Vector3.up * 100, Color.yellow, Mathf.Infinity);
            }
            teleportAround.Translate((player.position - teleportAround.position).normalized * 0.12f);
        }
    }

    void StartHunting()
    {
        int iterationLimit = 1000;
        int iterations = 0;
        while (!RandomTeleportChasingObject(30f) && iterations++ < iterationLimit) ;
        scaryNoiseSource.Play();

        if (iterations > 999) Debug.LogWarning("Iteration limit reached for RandomTeleportChasingObject");

        caughtCollider.enabled = true;
        hunting = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        StartCoroutine(CatchSequence());

    }

    IEnumerator CatchSequence()
    {
        CameraController _camera = FindObjectOfType<CameraController>();

        _camera.enabled = false;
        scareImage.SetActive(true);
        caughtCollider.enabled = false;
        hunting = false;
        scaryNoiseSource.Stop();
        jumpscareNoiseSource.PlayOneShot(jumpscareNoise);

        yield return new WaitForSeconds(0.7f);

        _camera.enabled = true;
        scareImage.SetActive(false);
        shadowRealmController.TeleportToShadowRealm();
    }

    //makes sure there is enough space in 2 units of 5 directions
    bool isEnoughSpace(Vector3 targetPosition, Vector3 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
      
        return new[] { Vector3.up, Vector3.left, Vector3.right, Vector3.forward, Vector3.back }
                    .Select(direction => rotation * direction)
                    .Select(direction => { Debug.DrawRay(targetPosition, direction * 2f, Color.yellow, 3f); return direction; })
                    .All(direction => !Physics.Raycast(targetPosition, direction, 2f));
    }

    bool IsNotNearPlayer(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, player.transform.position) > minimumPlayerTeleportDistance;
    }

    bool TryJumpRandomSurface()
    {
        RaycastHit hit;
        Vector3 rng = Random.onUnitSphere;
        Debug.DrawRay(player.position + Vector3.up * 2, rng * 100, Color.red, 3f);

        float jumpDistance = maxJumpRangeFromPlayer;
        if (isHarmless) jumpDistance = 3f;


        if (Physics.Raycast(player.position + Vector3.up * 2, rng, out hit, jumpDistance, surfacesMask)
            //&& !hit.transform.gameObject.CompareTag("SecurityCamera") //im just using this cause its not on anything else and Im too lazy to make a new one
            && Vector3.Angle(Vector3.up, hit.normal) < 10
            && IsNotNearPlayer(hit.point)
            && !los.isOnScreen(hit.point + hit.normal) 
            && isEnoughSpace(hit.point + hit.normal, hit.normal))
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            currentUp = hit.normal;
            transform.rotation = rotation;
            transform.position = hit.point;

            Vector3 monsterToPlayer = player.transform.position - transform.position;
            Vector3.OrthoNormalize(ref currentUp, ref monsterToPlayer);

            transform.rotation = Quaternion.LookRotation(monsterToPlayer);// * transform.rotation;

            return true;
        }
        return false;
    }

    bool RandomTeleportChasingObject(float teleportRange)
    {
        Vector3 randomCirclePointXY = Random.insideUnitCircle.normalized;
        Vector3 randomCirclePointXZ = new Vector3(randomCirclePointXY.x, 0f, randomCirclePointXY.y);
        Vector3 randomPoint = player.transform.position + randomCirclePointXZ * teleportRange;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas))
        {
            Debug.Log(hit.position);
            teleportAround.position = hit.position;
            return true;
        }

        return false;
    }

    public void SetActivelyHunting(bool activelyHunting)
    {
        if (activelyHunting)
        {
            enabled = true;
        }
        else
        {
            enabled = false;
            SendToTimeout(0f);
        }
    }

    public void SendToTimeout(float duration)
    {
        transform.position = timeOutSquare.position;
        StartCoroutine(Timeout(duration));
    }

    IEnumerator Timeout(float duration)
    {
        inTimeout = true;
        for (float time = 0f; time < duration; time += Time.deltaTime)
            yield return new WaitForFixedUpdate();
        inTimeout = false;
    }

    void Initialize(object sender, SaveEventArgs saveArgs)
    {
        SavePointID savepoint = saveArgs.savepoint;
        if(savepoint == SavePointID.research1)
        {
            SendToTimeout(10f);
            SetActivelyHunting(true);
            isHarmless = true;
        }
        else if (savepoint == SavePointID.research2)
        {
            SetActivelyHunting(true);
            isHarmless = false;
            InsanityMeter.Instance.setMinimumTimesCaught(1);
            
        }
        else if (savepoint == SavePointID.research3)
        {
            SetActivelyHunting(true);
            isHarmless = false;
            InsanityMeter.Instance.setMinimumTimesCaught(2);
        }
        else if (savepoint == SavePointID.research4)
        {
            SetActivelyHunting(false);
        }
        else if (savepoint == SavePointID.research5)
        {
            SetActivelyHunting(false);
        }
    }
}
