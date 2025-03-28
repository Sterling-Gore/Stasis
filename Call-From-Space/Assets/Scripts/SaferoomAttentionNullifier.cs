using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaferoomAttentionNullifier : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerAttention playerAttention;

    private void Start()
    {
        playerAttention = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttention>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerAttention.DisablePlayerAttention();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerAttention.EnablePlayerAttention();
    }
}
