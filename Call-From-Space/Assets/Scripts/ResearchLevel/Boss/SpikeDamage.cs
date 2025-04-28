using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float damageAmount = 10f;
    public float damageIncrease = 10f;
    public HealthSystem playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damageAmount);
            damageAmount += damageIncrease;

        }
    }
}
