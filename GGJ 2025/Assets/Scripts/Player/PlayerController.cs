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
    public Animator anim;

    public PlayerInputs inputs;
    private Rigidbody rb;
    public List<Rigidbody> rbRagdoll;
    private bool ragdollActive = false;

    [HideInInspector]
    public GameObject spawn;

    public Material material;

    public string team;
    public int id;

    [Header("Movement")]
    public float speed;
    public Vector3 movement;

    public float rotationSpeed;

    [Header("State Machine")]
    public PlayerState currentState;
    private float stateTimer;
    public float inputDelay;
    [Header("Kick")]
    public float kickTime;
    public GameObject kickHitbox;
    public float kickStrength;
    [Header("Push")]
    public float pushTime;
    public GameObject pushHitbox;
    public float pushStrength;
    public float pushSpeedMultiplier;
    [Header("Tackle")]
    public float tackleTime;
    public GameObject tackleHitbox;
    public float tackleStrength;
    public float tackleSpeed;
    [Range(0f, 1f)]
    public float tackleStopForce;
    [Header("Others")]
    public float slipTime;
    public float fallenTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        stateTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        inputs.GetInputs();

        StateMachine();

        Move();

        if(ragdollActive)
        {
            //AlignHipToPosition();
        }

    }

    private void OnEnable()
    {
        GoToSpawn();
    }

    public void Move()
    {
        

        if (ragdollActive)
        {
            rbRagdoll[0].AddForce(movement);
            rbRagdoll[1].AddForce(movement);
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.AddForce(movement);
        }

        movement = Vector3.zero;
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

                if (Time.time - stateTimer >= inputDelay)
                {
                    CanAction();
                }


                if (inputs.horizontal != 0 || inputs.vertical != 0)
                {
                    float saveTime = stateTimer;
                    ChangeState(PlayerState.Moving);
                    stateTimer = saveTime;
                }

                break;

            case PlayerState.Slipping:
                if (Time.time - stateTimer >= slipTime)
                {
                    DisableRagdoll();
                    ChangeState(PlayerState.Fallen);
                }
                break;

            case PlayerState.Moving:
                GetMovement();

                if (Time.time - stateTimer >= inputDelay)
                {
                    CanAction();
                }

                if (inputs.horizontal == 0 && inputs.vertical == 0)
                {
                    float saveTime = stateTimer;
                    ChangeState(PlayerState.Idle);
                    stateTimer = saveTime;
                }

                break;

            case PlayerState.Kicking:
                kickHitbox.SetActive(true);

                if (Time.time - stateTimer >= kickTime)
                {
                    ChangeState(PlayerState.Moving);
                }
                break;

            case PlayerState.Pushing:

                pushHitbox.SetActive(true);

                if (Time.time - stateTimer >= pushTime)
                {
                    bool pushed = pushHitbox.GetComponent<Push>().pushed;

                    if (pushed)
                    {
                        ChangeState(PlayerState.Moving);
                    }
                    else
                    {
                        ChangeState(PlayerState.Slipping);
                    }
                }
                break;

            case PlayerState.Tackling:

                tackleHitbox.SetActive(true);

                RotateAt(rb.velocity);

                if (Time.time - stateTimer >= tackleTime)
                {
                    rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0f, rb.velocity.y, 0f), tackleStopForce);
                    ChangeState(PlayerState.Moving);
                }
                break;
            case PlayerState.Fallen:
                if (Time.time - stateTimer >= fallenTime)
                {
                    ChangeState(PlayerState.Moving);
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

        switch(currentState)
        {
            case PlayerState.Slipping:
                SoundManager.instance.PlaySfx(SFX.Slip,true);
                EnableRagdoll();
                break;
            case PlayerState.Tackling:
                rb.AddForce(transform.forward * tackleSpeed, ForceMode.Impulse);
                ChangeAnimation("Tackle");
                break;
            case PlayerState.Fallen:
                ChangeAnimation("Standup");
                break;
            case PlayerState.Moving:
                ChangeAnimation("Run");
                break;
            case PlayerState.Idle:
                ChangeAnimation("Idle");
                break;
            case PlayerState.Kicking:
                ChangeAnimation("Kick");
                break;
            case PlayerState.Pushing:
                ChangeAnimation("Push");
                break;
        }
    }

    public void GoToSpawn()
    {
        if (spawn != null)
        {
            DisableRagdoll();

            transform.position = spawn.transform.position;
            transform.rotation = spawn.transform.rotation;

            rb.velocity = Vector3.zero;
            movement = rb.velocity;

            ChangeState(PlayerState.Idle);

            ChangeAnimation("Run");
        }
    }

    public void DisableRagdoll()
    {
        anim.enabled = true;
        GetComponent<CapsuleCollider>().isTrigger = false;

        AlignPositionToHip();

        for (int i = 0; i < rbRagdoll.Count; i++)
        {
            rbRagdoll[i].isKinematic = true;
        }
        ragdollActive = false;
    }

    public void EnableRagdoll()
    {
        AlignPositionToHip();

        anim.enabled = false;
        GetComponent<CapsuleCollider>().isTrigger = true;

        for (int i = 0; i < rbRagdoll.Count; i++)
        {
            rbRagdoll[i].isKinematic = false;
        }

        rbRagdoll[0].velocity = rb.velocity * 3;
        rbRagdoll[1].velocity = rb.velocity * 3;

        ragdollActive = true;
    }

    public void AlignPositionToHip()
    {
        Vector3 originalHipPos = rbRagdoll[0].transform.position;
        Vector3 originalChestPos = rbRagdoll[1].transform.position;
        transform.position = originalHipPos;

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        rbRagdoll[0].transform.position = originalHipPos;
        rbRagdoll[1].transform.position = originalChestPos;
    }

    public void AlignHipToPosition()
    {
        Vector3 originalHipPos = rbRagdoll[0].transform.localPosition;
        Vector3 originalChestPos = rbRagdoll[1].transform.localPosition;

        rbRagdoll[0].transform.localPosition = new Vector3(0f, originalHipPos.y, 0f);
        rbRagdoll[1].transform.localPosition = new Vector3(0f, originalChestPos.y, 0f);
    }

    public void ChangeAnimation(string animation)
    {
        anim.SetTrigger("Trigger" + animation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Push"))
        {
            if (currentState != PlayerState.Slipping && currentState != PlayerState.Fallen)
            {
                ChangeState(PlayerState.Slipping);
            }
            else
            {
                SoundManager.instance.PlaySfx(SFX.SoftImpact, true);
            }

            float speedForce = other.transform.parent.GetComponent<Rigidbody>().velocity.magnitude;
            speedForce *= pushSpeedMultiplier;

            Vector3 force = other.transform.forward * (pushStrength + speedForce);

            rb.AddForce(force, ForceMode.Impulse);

            if (ragdollActive)
                force *= 10;

            rbRagdoll[0].AddForce(force , ForceMode.Impulse);
            rbRagdoll[1].AddForce(force, ForceMode.Impulse);
        }
        else if(other.CompareTag("Tackle"))
        {
            rb.AddForce(other.transform.forward * tackleStrength, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.PlaySfx(SFX.SoftImpact, true);
        }
    }

}
