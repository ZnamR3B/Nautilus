using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Cannon : MonoBehaviour, I_OnBeforeAction
{
    public void onBeforeAction(BattleEntity user)
    {
        user.extraRange += 1;
    }
}
