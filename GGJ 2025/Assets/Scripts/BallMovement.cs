using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float kickStrength = 0;

    private Vector3 spawnPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        spawnPoint = transform.position;
    }

    public void Respawn()
    {
        transform.position = spawnPoint;
        rb.velocity = Vector3.zero;
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
    }
}
