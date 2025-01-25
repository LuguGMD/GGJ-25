using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float kickStrength = 0;

    private float startSpeed = 1.6f;

    private Vector3 spawnPoint;

    public GameObject lastTouch;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        spawnPoint = transform.position;
    }

    private void OnEnable()
    {
        Actions.instance.restartedMatch += LaunchToTeam;
    }

    private void OnDisable()
    {
        Actions.instance.restartedMatch -= LaunchToTeam;
    }

    public void Respawn()
    { 
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = spawnPoint;
        SoundManager.instance.PlaySfx(SFX.Spawn, true);
    }

    public void LaunchToTeam(string team)
    {
        float force = team == "Red" ? startSpeed : -startSpeed;
        rb.AddForce(Vector3.right * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kick"))
        {
            if(kickStrength == 0)
            {
                kickStrength = other.transform.parent.GetComponent<PlayerController>().kickStrength;
            }

            lastTouch = other.gameObject;
            rb.AddForce(other.transform.forward * (kickStrength + other.transform.parent.GetComponent<Rigidbody>().velocity.magnitude), ForceMode.Impulse);

            SoundManager.instance.PlaySfx(SFX.Kick, true);
        }



    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SoundManager.instance.PlaySfx(SFX.SoftImpact, true);
        }
        else if(collision.gameObject.CompareTag("Player"))
        {
            lastTouch = collision.gameObject;
        }
    }
}
