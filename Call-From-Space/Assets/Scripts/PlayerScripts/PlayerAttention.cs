using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttention : MonoBehaviour
{

    public Player_Movement.MovementStates currentMovementState;
    int currentMovementStateValue;

    int soundTypeValue;
    GameObject pathfinder;
    AlienController alienController;
    Player_Movement mover;
    speedometer speed;
    float movementChangeDelay;

    IEnumerator MovementAttentionHolder;

    Dictionary<Player_Movement.MovementStates, int> movementAttentionValues = new Dictionary<Player_Movement.MovementStates, int>
                                                    {
                                                        {Player_Movement.MovementStates.still, 0 },
                                                        {Player_Movement.MovementStates.crouch, 7 },
                                                        {Player_Movement.MovementStates.walk, 15 },
                                                        {Player_Movement.MovementStates.sprint, 30 }
                                                    };

    private void Awake()
    {
        alienController = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienController>();
        AlienAttentionHandler.Reload();
    }

    // Start is called before the first frame update
    void Start()
    {
        MovementAttentionHolder = MovementAttention();
        StartCoroutine(MovementAttentionHolder);
        mover = GetComponent<Player_Movement>();
        speed = GetComponent<speedometer>();
        movementChangeDelay = 0;
    }
    public void DisablePlayerAttention()
    {
        StopCoroutine(MovementAttentionHolder);
    }

    public void EnablePlayerAttention()
    {
        StartCoroutine(MovementAttentionHolder);
    }

    private void Update()
    {
        if(speed.speed < 1)
            ChangeSoundType(Player_Movement.MovementStates.still);
        else
            ChangeSoundType(mover.movementstate);
    }
    // Update is called once per frame
    public void ChangeSoundType(Player_Movement.MovementStates target)
    {
        currentMovementState = target;
        soundTypeValue = movementAttentionValues[target];
    }

    IEnumerator MovementAttention()
    {
        while (true)
        {
            Vector3 alienPosition = alienController.gameObject.transform.position;
            //just some random equation i made so attention it realisticly hears u more when you're are closer
            //int attentionIncrease = soundTypeValue - ((int)Math.Pow(Vector3.Distance(alienPosition, transform.position), 2)) / (5*soundTypeValue);
            AlienAttentionHandler.NoiseToAttentionIncrease(soundTypeValue, transform.position);

            //Debug.Log(attentionIncrease);
            //alienController.IncreaseAttention(attentionIncrease, this.transform.position);
            yield return new WaitForSeconds(alienController.currentAttentionTickRate);
        }
    }
}
