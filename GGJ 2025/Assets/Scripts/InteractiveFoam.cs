using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InteractiveFoam : MonoBehaviour
{
    private VisualEffect visualEffect;
    private Transform[] playerTransforms;

    // Start is called before the first frame update
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        List<PlayerController> players = GameManager.instance.players;

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                playerTransforms[i] = players[i].transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerTransforms.Length; i++)
        {
            if (playerTransforms[i] != null)
            {
                Vector3 playerPosInVFXCoordinates = playerTransforms[i].position - transform.position;
                visualEffect.SetVector3($"Player{i + 1}Position", playerPosInVFXCoordinates);
            }
        }
    }
}
