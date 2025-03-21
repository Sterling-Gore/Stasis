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

    IEnumerator InSoundBubbleHolder;

    Dictionary<Player_Movement.MovementStates, int> movementAttentionValues = new Dictionary<Player_Movement.MovementStates, int>
                                                    {
                                                        {Player_Movement.MovementStates.still, 0 },
                                                        {Player_Movement.MovementStates.crouch, 5 },
                                                        {Player_Movement.MovementStates.walk, 15 },
                                                        {Player_Movement.MovementStates.sprint, 30 }
                                                    };

    // Start is called before the first frame update
    void Start()
    {
        alienController = GameObject.FindGameObjectWithTag("Alien").GetComponent<AlienController>();
        InSoundBubbleHolder = InSoundBubble();
        StartCoroutine(InSoundBubbleHolder);
        mover = GetComponent<Player_Movement>();
        speed = GetComponent<speedometer>();
        movementChangeDelay = 0;
    }
    private void Update()
    {
        if(speed.speed < 1)
            changeSoundType(Player_Movement.MovementStates.still);
        else
            changeSoundType(mover.movementstate);
    }
    // Update is called once per frame
    public void changeSoundType(Player_Movement.MovementStates target)
    {
        currentMovementState = target;
        soundTypeValue = movementAttentionValues[target];
    }

    int checkForWallDampening(int attentionIncrease, Vector3 alienPosition)
    {
        RaycastHit[] hits;
        LayerMask wallMask = LayerMask.GetMask("Surfaces");

        hits = Physics.RaycastAll(this.transform.position,
            (alienPosition - this.transform.position).normalized,
            Vector3.Distance(this.transform.position, alienPosition),
            wallMask);

        int dampenedAttention = hits.Length > 0 ? (int)(attentionIncrease * (0.5 / hits.Length)) : attentionIncrease;
        Debug.Log("# walls: " + hits.Length + " | original attention: " + attentionIncrease + " | modified attention: " + dampenedAttention);

        return dampenedAttention;
    }
    IEnumerator InSoundBubble()
    {
        while (true)
        {
            Vector3 alienPosition = alienController.gameObject.transform.position;
            //just some random equation i made so attention it realisticly hears u more when you're are closer
            //int attentionIncrease = soundTypeValue - ((int)Math.Pow(Vector3.Distance(alienPosition, transform.position), 2)) / (5*soundTypeValue);
            int attentionIncrease = (int)(soundTypeValue * (soundTypeValue / Vector3.Distance(alienPosition, transform.position)));
            attentionIncrease = checkForWallDampening(attentionIncrease, alienPosition);
            attentionIncrease = Mathf.Clamp(attentionIncrease, 0, 100);

            //Debug.Log(attentionIncrease);
            alienController.IncreaseAttention(attentionIncrease, this.transform.position);
            yield return new WaitForSeconds(alienController.currentAttentionTickRate);
        }
    }
}
