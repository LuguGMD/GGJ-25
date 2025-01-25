using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public PlayerController pc;

    public List<SkinnedMeshRenderer> mesh;

    // Start is called before the first frame update
    public void ChangeColor()
    {
        for (int i = 0; i < mesh.Count; i++)
        {
            mesh[i].materials = new Material[3] { mesh[i].material, mesh[i].materials[2], pc.material };
        }
    }

}
