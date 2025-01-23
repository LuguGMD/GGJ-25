using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if(CompareTag("RedGate"))
            {
                player.team = "Red";
            }
            else
            {
                player.team = "Blue";
            }

            TeamManager.instance.AddTeam(player.inputs.playerType, player.team);
            other.gameObject.SetActive(false);
        }
    }
}
