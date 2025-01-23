using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    public List<Player> redTeam;
    public List<Player> blueTeam;

    public float startTime;
    public int matchScene;
    public int menuScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RemoveTeam(Player player)
    {
        StopAllCoroutines();

        if (redTeam.Contains(player)) redTeam.Remove(player);
        if(blueTeam.Contains(player)) blueTeam.Remove(player);
    }

    public void AddTeam(Player player, string team)
    {
        StopAllCoroutines();

        if (team == "Red")
        {
            redTeam.Add(player);
        }
        else
        {
            blueTeam.Add(player);
        }

        if (blueTeam.Count > 0 && redTeam.Count > 0 && redTeam.Count + blueTeam.Count >= GameManager.instance.players.Count)
        {
            StartCoroutine(nameof(StartMatch));
        }
        
    }

    public IEnumerator StartMatch()
    {
        for (int i = 0; i < startTime; i++) 
        {
            Debug.Log(5-i);
            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene(matchScene);
        
    }

}
