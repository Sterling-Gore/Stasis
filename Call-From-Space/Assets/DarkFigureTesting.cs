using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Device;
using UnityEngine.UIElements;
using Application = UnityEngine.Application;
using Screen = UnityEngine.Screen;

public class DarkFigureTesting : MonoBehaviour
{
    [SerializeField] Transform player, teleportAround;
    LOSChecker los;
    NavMeshAgent NMA;
    
    // Start is called before the first frame update
    void Start()
    {
        los = GetComponentInChildren<LOSChecker>();
        NMA = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.position);

        //if (!los.visibleAndOnScreen)
        //{
        //    int rng = Random.Range(1, 200);
        //    if (rng <= 1)
        //    {
        //        NothingPersonal();
        //        Debug.Log("woosh");
        //    }
        //}

        Vector3 position = transform.position;
        float range = 0.3f;

        Vector3 randomCirclePointXY = Random.insideUnitCircle;
        Vector3 randomCirclePointXZ = new Vector3(randomCirclePointXY.x, 0f, randomCirclePointXY.y);
        Vector3 randomPoint = teleportAround.position + randomCirclePointXZ * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 5f, NavMesh.AllAreas))
        {
            NMA.Warp(hit.position);
            //Debug.DrawLine(randomPoint, randomPoint + Vector3.up * 100, Color.yellow, Mathf.Infinity);
        }

        //teleportAround.Translate((player.position - teleportAround.position).normalized * 0.025f);
        //CheckLineOfSight();
    }

    void NothingPersonal()
    {
        teleportAround.position = (player.transform.position + player.rotation * Vector3.back * 2) ;
    }
}
