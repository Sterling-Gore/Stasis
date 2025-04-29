using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeStabber : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator spikeStabAnimation;
    public Transform PlayerPosition;
    public bool isActive = false;

    [Header("Cooldown")] 
    public bool spikeUnderCoolDown = false;
    public float spikeCoolDownTimer = 9f;
    public float spikeCoolDown = 0f;

    [Header("Camera Shake")]
    public CameraShakeGeneral cameraShake;

    [Header("Spike Attack Stats")]
    public bool currentyAttacking = false;
    public float initialRumbleWaitPeriod = 2f;
    public float underPlayerWaitPeriod = 3f;
    public float timeAfterStabbing = 1.5f;
    public Collider damageCollider;

    [Header("Attack Chance")]
    public float percentageOfAttacking = .5f;
    public float timeToWaitBetweenEachChance = .1f;
    public float timerBetweenEachChance = 0f;




    void Start()
    {
        damageCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!currentyAttacking && isActive)
        {
            if(spikeUnderCoolDown)
            {
                if(spikeCoolDown < spikeCoolDownTimer)
                {
                    spikeCoolDown += Time.deltaTime;
                }
                else
                {
                    spikeCoolDown = 0f;
                    spikeUnderCoolDown = false;
                }
            }
            else
            {
                if(timerBetweenEachChance > timeToWaitBetweenEachChance)
                {
                    if(percentageOfAttacking >= Random.value)
                    {
                        timerBetweenEachChance = 0f;
                        currentyAttacking = true;
                        StartCoroutine(attack());
                    }
                }
                else
                {
                    timerBetweenEachChance += Time.deltaTime;
                }
            }
        }
    }

    

    IEnumerator attack()
    {
        //start audio
        cameraShake.StartShake(initialRumbleWaitPeriod+.5f, 0.1f);
        yield return new WaitForSeconds(initialRumbleWaitPeriod);
        // strengthen audio
        //cameraShake.StartShake(0.2f + underPlayerWaitPeriod, .1f);

        //teleport under player
        transform.position = new Vector3(PlayerPosition.position.x + 2.05f, PlayerPosition.position.y - 14.2f, PlayerPosition.position.z + .67f);
        spikeStabAnimation.SetTrigger("Pre_Stab");



        yield return new WaitForSeconds(underPlayerWaitPeriod);
        //stab audio
        damageCollider.enabled = true;
        spikeStabAnimation.SetTrigger("Stab");
        yield return new WaitForSeconds(0.2f);
        damageCollider.enabled = false;


        yield return new WaitForSeconds(timeAfterStabbing);
        spikeStabAnimation.SetTrigger("Hidden");
        yield return new WaitForSeconds(0.2f);
        
        
        yield return new WaitForSeconds(1f);

        currentyAttacking = false;
        spikeUnderCoolDown = true;
    }
}
