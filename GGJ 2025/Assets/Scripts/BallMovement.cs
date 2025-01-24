using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float kickStrength = 0;

    private Vector3 spawnPoint;

    public GameObject lastTouch;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        spawnPoint = transform.position;
    }

    public void Respawn()
    { 
        rb.velocity = Vector3.zero;
        transform.position = spawnPoint;
        SoundManager.instance.PlaySfx(SFX.Spawn, true);
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

            rb.AddForce(other.transform.forward * kickStrength, ForceMode.Impulse);

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
