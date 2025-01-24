using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    public float scaleSize = 1.2f;
    public float returnSpeed = 6f;

    public GameObject mesh;

    private void Update()
    {
        mesh.transform.localScale = Vector3.Lerp(mesh.transform.localScale, Vector3.one, returnSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        mesh.transform.localScale = Vector3.one * scaleSize;
    }
}
