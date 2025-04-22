using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Device;
using UnityEngine.UIElements;
using Application = UnityEngine.Application;
using Screen = UnityEngine.Screen;

public class DarkFigureTesting : MonoBehaviour
{
    [SerializeField] Transform player, teleportAround, darkFigureHead, LOSPointsTransform;

    [SerializeField]
    Transform[] losPoints;

    ShadowRealm shadowRealmController;
    LOSChecker los;
    NavMeshAgent NMA;
    Collider caughtCollider;
    bool hunting;
    LayerMask surfacesMask;

    Vector3 currentUp;
    [SerializeField] float minimumPlayerTeleportDistance;

    // Start is called before the first frame update
    void Start()
    {
        los = GetComponentInChildren<LOSChecker>();
        NMA = GetComponent<NavMeshAgent>();
        InsanityMeter.Instance.MaxInsanity += StartHunting;
        shadowRealmController = FindObjectOfType<ShadowRealm>();
        caughtCollider = GetComponent<Collider>();
        caughtCollider.enabled = true;
        hunting = false;
        surfacesMask = LayerMask.GetMask("Surfaces");
        currentUp = Vector3.up;
        
        losPoints = LOSPointsTransform.Cast<Transform>().ToArray();

        Vector3 test = Quaternion.FromToRotation(Vector3.up, Vector3.forward) * Vector3.forward;
        Debug.Log(test);
    }

    // Update is called once per frame
    void Update()
    {
        darkFigureHead.LookAt(player.position+Vector3.up*2);


        if (losPoints.All(point => !los.isOnScreen(point.position)))
        {
            int rng = Random.Range(1, 200);
            if (rng == 1)
            {
                while (!FindRandomSurface()) ;
            }
            //Debug.Log("Where is it");
        }
        //else
            //Debug.Log("I See it");



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
            teleportAround.Translate((player.position - teleportAround.position).normalized * 0.025f);
        }

    }

    void NothingPersonal()
    {
        Vector3 proposedPosition = (player.transform.position + player.rotation * Vector3.back * 3);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(proposedPosition, out hit, 1f, NavMesh.AllAreas))
        {
            NMA.Warp(hit.position);
            transform.LookAt(player.position);
            //Debug.DrawLine(randomPoint, randomPoint + Vector3.up * 100, Color.yellow, Mathf.Infinity);
        }
    }

    void StartHunting()
    {
        caughtCollider.enabled = true;
        hunting = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        shadowRealmController.TeleportToShadowRealm();
        caughtCollider.enabled = false;
        hunting = false;
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

    bool FindRandomSurface()
    {
        RaycastHit hit;
        Vector3 rng = Random.onUnitSphere;
        Debug.DrawRay(player.position + Vector3.up * 2, rng * 100, Color.red, 3f);
        if (Physics.Raycast(player.position + Vector3.up * 2, rng, out hit, Mathf.Infinity, surfacesMask) 
            && !los.isOnScreen(hit.point + hit.normal) 
            && isEnoughSpace(hit.point + hit.normal, hit.normal)
            && IsNotNearPlayer(hit.point))
        {
            Debug.Log(hit.point + " : " + hit.normal);

            Vector3 offset = (hit.normal == Vector3.up) ? Vector3.zero : -hit.normal;

            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            currentUp = hit.normal;
            transform.rotation = rotation;
            transform.position = hit.point + offset;

            Vector3 monsterToPlayer = player.transform.position - transform.position;
            Vector3.OrthoNormalize(ref currentUp, ref monsterToPlayer);

            float angle = -Vector3.Angle(monsterToPlayer, transform.forward);

            Debug.Log("before angle: " + transform.rotation.eulerAngles + " | " + "angle calc: " + angle);

            transform.rotation = Quaternion.AngleAxis(angle, hit.normal) * transform.rotation;

            //transform.LookAt(player.transform.position + Vector3.up);

            //transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, hit.normal) * transform.rotation;
            return true;
        }
        return false;
    }
}
