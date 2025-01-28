using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamParticlesDespawner : MonoBehaviour
{
    [SerializeField]
    private float despawnProbability;

    private void OnParticleCollision(GameObject particle)
    {
        float random = Random.Range(0, 1);

        if (random < despawnProbability)
        {
            Destroy(particle);
        }

        
    }
}
