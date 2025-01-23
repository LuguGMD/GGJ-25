using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Moving,
    Slipping,
    Kicking,
    Pushing,
    Tackling,
    Fallen
}

[RequireComponent(typeof(PlayerInputs), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public PlayerInputs inputs;
    private Rigidbody rb;

    [HideInInspector]
    public GameObject spawn;

    public string team;

    [Header("Movement")]
    public float speed;
    public float maxSpeed;

    public float friction;

    public Vector3 knockback;
    public Vector3 movement;

    public float rotationSpeed;

    [Header("State Machine")]
    public PlayerState currentState;
    private float stateTimer;
    [Header("Kick")]
    public float kickTime;
    public GameObject kickHitbox;
    public float kickStrength;
    [Header("Push")]
    public float pushTime;
    public GameObject pushHitbox;
    public float pushStrength;
    [Header("Tackle")]
    public float tackleTime;
    public GameObject tackleHitbox;
    public float tackleStrength;
    public float tackleSpeed;
    [Header("Others")]
    public float slipTime;
    public float fallenTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        inputs.GetInputs();

        StateMachine();

        Move();

    }

    private void OnEnable()
    {
        GoToSpawn();
    }

    public void Move()
    {
        movement += rb.velocity;

        movement = Vector3.ClampMagnitude(movement, maxSpeed);

        movement = Vector3.Lerp(movement, new Vector3(0f, rb.velocity.y, 0f), friction * Time.deltaTime);

        movement += knockback;

        rb.velocity = movement;


        movement = Vector3.zero;
        knockback = Vector3.zero;
    }

    public void GetMovement()
    {
        movement = new Vector3(inputs.horizontal, 0f, inputs.vertical).normalized * speed * Time.deltaTime;

        RotateAt(movement);
    }

    public void RotateAt(Vector3 pos)
    {
        Quaternion t = transform.rotation;
        transform.LookAt(transform.position + new Vector3(pos.x, 0f, pos.z));

        transform.Rotate(new Vector3(0, transform.rotation.y, 0));

        t = Quaternion.Lerp(t, transform.rotation, rotationSpeed * Time.deltaTime);

        transform.rotation = t;
    }

    public void CanAction()
    {
        if (inputs.kick) ChangeState(PlayerState.Kicking);
        if (inputs.push) ChangeState(PlayerState.Pushing);
        if (inputs.tackle) ChangeState(PlayerState.Tackling);
    }

    public void StateMachine()
    {
        switch(currentState) 
        {
            case PlayerState.Idle:
                GetMovement();
                CanAction();


                if (inputs.horizontal != 0 || inputs.vertical != 0)
                {
                    ChangeState(PlayerState.Moving);
                }

                break;

            case PlayerState.Slipping:
                if (Time.time - stateTimer >= slipTime)
                {
                    ChangeState(PlayerState.Fallen);
                }
                break;

            case PlayerState.Moving:
                GetMovement();
                CanAction();

                if (inputs.horizontal == 0 && inputs.vertical == 0)
                {
                    ChangeState(PlayerState.Idle);
                }

                break;

            case PlayerState.Kicking:
                kickHitbox.SetActive(true);

                if (Time.time - stateTimer >= kickTime)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;

            case PlayerState.Pushing:

                pushHitbox.SetActive(true);

                if (Time.time - stateTimer >= pushTime)
                {
                    bool pushed = pushHitbox.GetComponent<Push>().pushed;

                    if (pushed)
                    {
                        ChangeState(PlayerState.Idle);
                    }
                    else
                    {
                        ChangeState(PlayerState.Slipping);
                    }
                }
                break;

            case PlayerState.Tackling:

                tackleHitbox.SetActive(true);

                movement += transform.forward * tackleSpeed * Time.deltaTime;

                RotateAt(movement);

                if (Time.time - stateTimer >= tackleTime)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;
            case PlayerState.Fallen:
                if (Time.time - stateTimer >= fallenTime)
                {
                    ChangeState(PlayerState.Idle);
                }
                break;
        }
    }

    public void ChangeState(PlayerState state)
    {
        kickHitbox.SetActive(false);
        pushHitbox.SetActive(false);
        tackleHitbox.SetActive(false);

        currentState = state;
        stateTimer = Time.time;
    }

    public void GoToSpawn()
    {
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            transform.rotation = spawn.transform.rotation;

            rb.velocity = Vector3.zero;
            movement = rb.velocity;
            knockback = rb.velocity;

            ChangeState(PlayerState.Idle);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Push"))
        {
            if (currentState != PlayerState.Slipping && currentState != PlayerState.Fallen)
            {
                ChangeState(PlayerState.Slipping);
            }
            knockback += other.transform.forward * pushStrength;
        }
        else if(other.CompareTag("Tackle"))
        {
            knockback += other.transform.forward * tackleStrength;
        }
    }

}
