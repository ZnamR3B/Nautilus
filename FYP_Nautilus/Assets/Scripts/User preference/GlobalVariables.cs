using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameSpeed { slow, normal, fast};
public static class GlobalVariables
{
    public static float duration_HPReduce
    {
        get
        {
            if (gameSpeed == GameSpeed.slow)
                return 1.5f;
            else if (gameSpeed == GameSpeed.normal)
                return .5f;
            else
                return .25f;
        }
    }
    public static float duration_dialog 
    { 
        get 
        {
            if (gameSpeed == GameSpeed.slow)
                return 2;
            else if (gameSpeed == GameSpeed.normal)
                return 1;
            else
                return .5f;
        } 
    }

    public static GameSpeed gameSpeed = GameSpeed.fast;
}
