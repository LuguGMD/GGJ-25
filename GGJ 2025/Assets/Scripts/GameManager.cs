using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<PlayerController> players;
    public GameObject playerPrefab;
    private int maxPlayers = 4;

    public List<GameObject> spawnPoints;
    public int blueTeamStartIndex = 3;

    [Header("Match")]
    public bool inMatch = false;
    public int bluePoints;
    public int redPoints;

    public List<Material> blueColors;
    public List<Material> redColors;

    public TextMeshProUGUI bluePointsText;
    public TextMeshProUGUI redPointsText;

    public int pointsToWin = 3;


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

    private void Start()
    {
        if(inMatch)
        {
            for (int i = 0; i < TeamManager.instance.redTeam.Count; i++)
            {
                SpawnPlayer(TeamManager.instance.redTeam[i], i, "Red", i);
            }

            for (int i = 0; i < TeamManager.instance.blueTeam.Count; i++)
            {
                SpawnPlayer(TeamManager.instance.blueTeam[i], blueTeamStartIndex + i, "Blue", i);
            }
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
                SpawnPlayer((Player)Enum.Parse(typeof(Player), player), players.Count);
            }

        }
    }

    public void SpawnPlayer(Player playerType, int spawnInd, string team = "", int id = 0)
    {
        bool canCreate = true;

        TeamManager.instance.StopTimer();

        //Checking if player already exists
        for(int i = 0;i < players.Count; ++i)
        {
            if (players[i].inputs.playerType == playerType)
            {
                canCreate = false;

                if (!players[i].gameObject.activeSelf)
                {
                    players[i].GoToSpawn();
                    players[i].gameObject.SetActive(true);
                    players[i].team = "";
                    players[i].id = id;
                    TeamManager.instance.RemoveTeam(playerType);
                }

            }
        }

        if(canCreate)
        {
            //Getting spawn point
            Transform spawn = spawnPoints[spawnInd].transform;
            //Creating player
            players.Add(Instantiate(playerPrefab, spawn.position, spawn.rotation).GetComponent<PlayerController>());
            //Saving spawn point
            players[players.Count-1].spawn = spawn.gameObject;
            //Passing player type
            players[players.Count-1].inputs.playerType = playerType;

            if (team != "")
            {
                players[players.Count - 1].team = team;
                players[players.Count - 1].id = id;

                Material mat = redColors[id];

                if (team == "Blue") mat = blueColors[id];

                players[players.Count - 1].material = mat;

                players[players.Count - 1].GetComponent<PlayerColor>().ChangeColor();
            }

            Actions.instance.addPlayerAction?.Invoke();

        }
    }

    public void Score(string team)
    {
        if(team == "Blue")
        {
            redPoints++;
            redPointsText.text = redPoints.ToString();
        }
        else if(team == "Red")
        {
            bluePoints++;
            bluePointsText.text = bluePoints.ToString();
        }

        if(bluePoints >= pointsToWin || redPoints >= pointsToWin)
        {
            TeamManager.instance.Invoke(nameof(TeamManager.instance.EndMatch), 0.5f);
        }

    }

}
