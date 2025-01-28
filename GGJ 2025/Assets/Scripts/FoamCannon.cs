using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamCannon : MonoBehaviour
{
    public string side;
    public ParticleSystem vfx;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        ShootCannon("Red");
    //    }
    //}

    private void OnEnable()
    {
        Actions.instance.goalScored += ShootCannon;
    }

    private void OnDisable()
    {
        Actions.instance.goalScored -= ShootCannon;
    }

    public void ShootCannon(string team)
    {
        if (side == team)
        {
            animator.SetBool("Spin", true);
            vfx.Play();
        }
    }

    public void RestartCannonPosition(string team)
    {
        if (side == team)
        {
            animator.SetBool("Spin", false);
        }
    }
}
