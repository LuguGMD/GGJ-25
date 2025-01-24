using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float kickStrength = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        }
    }
}
