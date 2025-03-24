using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAttackHitbox : MonoBehaviour
{
    [SerializeField]
    float cooldown, damage;

    float timer;

    AlienController alien;

    HealthSystem playerHealth;
    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        alien = GetComponentInParent<AlienController>();
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
            if (alien.currentState != AlienController.State.Hunting)
            {
                alien.IncreaseAttention(100, other.transform.position);
            }
            else
            {
                playerHealth.TakeDamage(damage);
            }
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
