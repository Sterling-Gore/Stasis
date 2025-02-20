using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDev.Scripts.Oxygen;
using Newtonsoft.Json.Linq;
using UnityEngine.Audio;
using System.Linq;
using UnityEngine.SceneManagement; // Replace 'GameDev' with your actual project root namespace

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    public float walk_speed = 2f;
    public float sprint_speed = 2f;
    public float crouch_speed = 2f;
    float current_speed;
    float yscale;
    float HorizInput;
    float VertInput;
    Vector3 moveDirection;
    public Rigidbody rb;
    public float groundDrag;
    public bool MovementIsLocked;
    public Collider CrouchCollider;

    [Header("Camera attributes")]
    public Transform orientation;

    [Header("Slope handler")]
    public float maxSlope;
    public float playerHeight = 5;
    private RaycastHit slopeHit;

    public LayerMask PlayerLayer;

    [Header("Interactor")]
    Interactor interactor;

    public enum MovementStates
    {
        walk,
        sprint,
        crouch
    }

    public MovementStates movementstate;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        yscale = transform.localScale.y;

        MovementIsLocked = false;
        movementstate = MovementStates.walk;
        current_speed = walk_speed;

        PlayerLayer = ~(PlayerLayer);

        interactor = gameObject.GetComponent<Interactor>();

    }

    void FixedUpdate()
    {
        if (!MovementIsLocked)
            MovePlayer();
    }

    void Update()
    {
        MyInput();
        SpeedControl();
        rb.drag = groundDrag;
        MovementIsLocked = interactor.inUI;

        /*if (oxygenSystem != null)
        {
            // Adjust oxygen based on movement
            if (rb.velocity.magnitude > 0.1f) // Check if the player is moving
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Running
                    if (oxygenSystem.LosingOxygen)
                        oxygenSystem.DecreaseOxygen(runningOxygenCost);
                    SoundSourcesController.instance.CreateNewSoundSource(transform.position, runningSoundRadius);
                }
                else
                {
                    // Walking
                    if (oxygenSystem.LosingOxygen)
                        oxygenSystem.DecreaseOxygen(walkingOxygenCost);
                    SoundSourcesController.instance.CreateNewSoundSource(transform.position, walkingSoundRadius);
                }
            }
            else
            {
                // not moving
                if (oxygenSystem.LosingOxygen)
                    oxygenSystem.DecreaseOxygen(walkingOxygenCost);
            }

        }*/
    }

    void MyInput()
    {
        HorizInput = Input.GetAxisRaw("Horizontal");
        VertInput = Input.GetAxisRaw("Vertical");

        if (!MovementIsLocked)
        {
            if (Input.GetKeyDown("left shift"))
            {
                //if im crouched
                if (movementstate == MovementStates.crouch)
                {
                    //if I can stand up
                    bool standUp = ToggleCrouchCollider();
                    if (standUp)
                        movementstate = MovementStates.sprint;
                }
                else
                    movementstate = MovementStates.sprint;
            }
            else if (Input.GetKeyUp("left shift"))
            {
                if (movementstate == MovementStates.crouch)
                {
                    //if I can stand up
                    bool standUp = ToggleCrouchCollider();
                    if (standUp)
                        movementstate = MovementStates.walk;
                }
                else
                    movementstate = MovementStates.walk;
            }
            else if (Input.GetKeyDown("left ctrl"))
            {
                bool standUp = ToggleCrouchCollider();
                movementstate = (movementstate == MovementStates.crouch && standUp) ? MovementStates.walk : MovementStates.crouch;
            }
    

        
        }
        
        

    }


    bool ToggleCrouchCollider()
    {
        RaycastHit hit;
        if (movementstate == MovementStates.crouch && !Physics.Raycast(transform.position, transform.up, out hit, 2.05f, PlayerLayer))
        {
            transform.localScale = new Vector3(transform.localScale.x, yscale, transform.localScale.z);
            return true;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, yscale / 2, transform.localScale.z);
            return false;

        }
        //transform.localScale = (movementstate == MovementStates.crouch) ? new Vector3(transform.localScale.x, yscale, transform.localScale.z) : new Vector3(transform.localScale.x, yscale / 2, transform.localScale.z);
    }



    void MovePlayer()
    {
        moveDirection = (orientation.forward * VertInput) + (orientation.right * HorizInput);
        
        if (OnSlope())
            rb.AddForce(GetSlopeMoveDirection() * current_speed * 10f, ForceMode.Force);
        else
        {
            rb.AddForce(moveDirection.normalized * current_speed * 10f, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    void SpeedControl()
    {
        switch(movementstate)
        {
            case MovementStates.walk:
                current_speed = walk_speed;
                break;
            case MovementStates.sprint:
                current_speed = sprint_speed;
                break;
            case MovementStates.crouch:
                current_speed = crouch_speed;
                break;
            default:
                current_speed = walk_speed;
                break;
                Debug.Log("ERROR: UNSPPORTED TYPE FOR MOVEMENT STATES IN PLAYER_MOVEMENT SCRIPT");
        }
        //limiting speed on slope
        if (OnSlope())
        {
            if (rb.velocity.magnitude > current_speed)
            {
                rb.velocity = rb.velocity.normalized * current_speed;
            }
        }
        else
        { //not on slope
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limit velocity
            if (flatVel.magnitude > current_speed)
            {
                Vector3 limitedVel = flatVel.normalized * current_speed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.1f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlope && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
