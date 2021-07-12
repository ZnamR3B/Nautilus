using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCondition_Faint : MonoBehaviour,I_OnBeforeAction, I_OnRoundEnd
{
    int count;

    public void initFaint(int round)
    {
        count = round;
    }
    public void onBeforeAction(BattleEntity user)
    {
        if(Random.Range(0,100) < 30)
        {
            user.gameObject.AddComponent<SpecialCondition_Faint>();
            user.GetComponent<SpecialCondition_Faint>().initFaint(1);
        }
    }

    public void onRoundEnd()
    {
        count--;
        if(count == 0)
        {
            Destroy(gameObject);
        }
    }
}
