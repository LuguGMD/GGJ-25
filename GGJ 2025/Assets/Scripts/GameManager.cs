using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<PlayerController> players;
    public GameObject playerPrefab;
    private int maxPlayers = 4;

    public List<GameObject> spawnPoints;

    public bool inMatch = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!inMatch)
        {
            AddPlayer();
        }
    }

    public void AddPlayer()
    {
        if(players.Count < maxPlayers)
        {
            string player = "";

            if(Input.GetKeyDown(KeyCode.F))
            {
                player = "K1";
            }
            if (Input.GetKeyDown(KeyCode.Comma))
            {
                player = "K2";
            }

            //Getting all inputs from controllers
            for (int i = 1; i <= 4; ++i)
            {
                for (int j = 0; j < 16; ++j) 
                {
                    string button = "Joystick" + i + "Button" + j;
                    if(Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode),button)))
                    {
                        player = "C" + i;
                    }
                } 
            }

            //A player pressed something
            if(player != "")
            {
                SpawnPlayer((Player)Enum.Parse(typeof(Player), player));
            }

        }
    }

    public void SpawnPlayer(Player playerType)
    {
        bool canCreate = true;

        //Checking if player already exists
        for(int i = 0;i < players.Count; ++i)
        {
            if (players[i].inputs.playerType == playerType)
            {
                canCreate = false;

                players[i].GoToSpawn();
                players[i].gameObject.SetActive(true);
                players[i].team = "";
                TeamManager.instance.RemoveTeam(playerType);

            }
        }

        if(canCreate)
        {
            //Getting spawn point
            Transform spawn = spawnPoints[players.Count].transform;
            //Creating player
            players.Add(Instantiate(playerPrefab, spawn.position, spawn.rotation).GetComponent<PlayerController>());
            //Saving spawn point
            players[players.Count-1].spawn = spawn.gameObject;
            //Passing player type
            players[players.Count-1].inputs.playerType = playerType;
        }
    }

}
