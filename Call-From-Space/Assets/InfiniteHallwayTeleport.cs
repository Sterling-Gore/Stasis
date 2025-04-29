using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteHallwayTeleport : MonoBehaviour
{
    [SerializeField]
    Transform teleportPoint;
    bool playedAudioAlready = false;

    public AudioSource runningInCircles;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if(!playedAudioAlready)
        {
            playedAudioAlready = true;
            runningInCircles.Play();
        }
        Vector3 relativePosition = transform.InverseTransformPoint(other.transform.position);

        other.transform.parent.GetComponent<Rigidbody>().position = teleportPoint.TransformPoint(relativePosition);
    }
}
