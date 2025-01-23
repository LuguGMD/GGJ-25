using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager instance;

    public List<Player> redTeam;
    public List<Player> blueTeam;

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
        if(redTeam.Contains(player)) redTeam.Remove(player);
        if(blueTeam.Contains(player)) blueTeam.Remove(player);
    }

    public void AddTeam(Player player, string team)
    {
        if (team == "Red")
        {
            redTeam.Add(player);
        }
        else
        {
            blueTeam.Add(player);
        }
        
    }

}
