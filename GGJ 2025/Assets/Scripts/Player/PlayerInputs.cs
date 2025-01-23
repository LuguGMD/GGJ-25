using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    C1,
    C2,
    C3,
    C4,
    K1,
    K2,

}
public class PlayerInputs : MonoBehaviour
{
    public Player playerType;

    [Header("Inputs")]
    public float horizontal;
    public float vertical;
    public bool kick;
    public bool tackle;
    public bool push;

    public void GetInputs()
    {
        horizontal = Input.GetAxis("Horizontal" + playerType.ToString());
        vertical = Input.GetAxis("Vertical" + playerType.ToString());
        kick = Input.GetButton("Kick" + playerType.ToString());
        push = Input.GetButton("Push" + playerType.ToString());
        tackle = Input.GetButton("Tackle" + playerType.ToString());
    }
}
