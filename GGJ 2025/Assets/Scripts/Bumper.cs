using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float impulseStrength;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ball"))
        {
            collision.rigidbody.AddForce(transform.forward * impulseStrength, ForceMode.Impulse);
        }
    }
}
