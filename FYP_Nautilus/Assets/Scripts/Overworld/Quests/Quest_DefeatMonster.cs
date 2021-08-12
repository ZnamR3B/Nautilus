using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_DefeatMonster : Quest, I_OnDefeatEnemy
{
    public int targetEnemyIndex;
    public int current;
    public int goal;
    public void defeatedEnemy(int index)
    {
        if(state == QuestState.unlocked && targetEnemyIndex == index)
        {
            current++;
            if (current >= goal)
            {
                //clear
            }
        }
    }

    public override string objectiveProgress()
    {
        return current + " / " + goal;
    }
}
