using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPanel : MonoBehaviour
{
    public GameObject iconImage;
    public GameObject instructionsText;

    private int playerNumber = 0;

    private void Start()
    {
        Actions.instance.addPlayerAction += AddPlayer;
    }

    private void OnDisable()
    {
        Actions.instance.addPlayerAction -= AddPlayer;
    }

    public void AddPlayer()
    {
        playerNumber++;
        Instantiate(iconImage, transform); 

        if(playerNumber >= 4)
        {
            Destroy(instructionsText);
        }
    }

}
