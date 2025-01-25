using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            other.GetComponent<BallMovement>().Respawn();
        }
        else if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GoToSpawn();
        }
    }
}
