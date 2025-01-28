using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public string team;

    public BallMovement ball;

    private float respawnTime = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            ball = other.GetComponent<BallMovement>();
            PlayerController player = ball.lastTouch.GetComponent<PlayerController>();

            ball.gameObject.SetActive(false);
            Invoke(nameof(RespawnBall), respawnTime);

            GameManager.instance.Score(team);
            Actions.instance.goalScored?.Invoke(team);
        }
    }

    public void RespawnBall()
    {
        ball.gameObject.SetActive(true);
        ball.Respawn();

        List<PlayerController> players = GameManager.instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].GoToSpawn();
        }

        Actions.instance.restartedMatch?.Invoke(team);

    }
}
