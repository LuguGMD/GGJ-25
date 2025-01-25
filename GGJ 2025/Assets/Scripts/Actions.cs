using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Actions
{
    private static Actions id;
    public static Actions instance
    {
        get {
            if(id == null)
            {
                id = new Actions();
            }
            return id; 
        }
        set {}
    }

    public Action addPlayerAction;
    public Action<string> goalScored;
    public Action<string> restartedMatch;
}
