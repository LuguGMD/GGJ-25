using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public string team;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            BallMovement ball = other.GetComponent<BallMovement>();
            PlayerController player = ball.lastTouch.GetComponent<PlayerController>();

            ball.Respawn();

            GameManager.instance.Score(team);
        }
    }
}
