using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAttackHitbox : MonoBehaviour
{
    [SerializeField]
    float cooldown, damage;

    float timer;

    HealthSystem playerHealth;
    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
    }

    private void Update()
    {
        if (timer >= 0)
            timer -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (timer <= 0)
        {
            playerHealth.TakeDamage(damage);
            timer = cooldown;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (timer <= 0)
        {
            playerHealth.TakeDamage(damage);
            timer = cooldown;
        }
    }
}
