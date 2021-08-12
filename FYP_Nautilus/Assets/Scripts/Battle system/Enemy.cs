using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleEntity
{
    public Element element;

    public int anger;
    public int angerLine;
    //public int[] angerLevel = new int[3];

    public EnemySkill[] enemySkills;
    public EnemySkill[] angerSkills;
    public void decideAction()
    {
        anger = 0;
        for (int i = 0; i < battleSystem.laneCount; i++)
        {
            anger += battleSystem.allyChar[i].hate;
        }
        if (anger >= angerLine)
        {
            //anger skill
            Action action;
            action.cancelled = false;
            action.user = this;
            action.skill = angerSkills[Random.Range(0, angerSkills.Length)];
            action.prio = action.skill.prio;
            action.ch = null;
            action.item = null;
            battleSystem.actions.Add(action);
        }
        else
        {
            //choose target character
            AllyCharacter target = null;
            int laneCount = battleSystem.laneCount;            
            if(anger > 0)
            {                
                int random = Random.Range(0, anger) + 1;
                for (int i = 0; i < laneCount; i++)
                {
                    if (random <= battleSystem.allyChar[i].hate)
                    {
                        target = battleSystem.allyChar[i];
                        break;
                    }
                    else
                    {
                        random -= battleSystem.allyChar[i].hate;
                    }
                }
            }
            else
            {
                target = battleSystem.allyChar[Random.Range(0, battleSystem.laneCount)];
            }
            List<EnemySkill> inRangeSkills = new List<EnemySkill>();
            //find skills can hit that specific target
            foreach (EnemySkill skill in enemySkills)
            {
                if (skill.targetInRange(this, target, battleSystem))
                {
                    inRangeSkills.Add(skill);
                }
            }
            Action action;
            action.cancelled = false;
            action.ch = null;
            action.item = null;
            action.user = this;
            if (inRangeSkills.Count > 0)
            {   
                action.skill = inRangeSkills[Random.Range(0, inRangeSkills.Count)];
                action.prio = action.skill.prio;                
            }
            else
            {
                action.skill = enemySkills[Random.Range(0, enemySkills.Length)];
                action.prio = action.skill.prio;
            }
            battleSystem.actions.Add(action);
        }
    }

    public override IEnumerator action(Action action)
    {
        if(canMove)
        {
            onBeforeAction(this);
            yield return StartCoroutine(action.skill.use(this));
            onAfterAction(this);
        }
        else
        {
            yield return StartCoroutine(GlobalMethods.printDialog(battleSystem.dialog, "But cannot move...", GlobalVariables.duration_dialog));
        }
    }

    public override IEnumerator defeat()
    {
        yield return StartCoroutine(GlobalMethods.printDialog(battleSystem.dialog, "it is defeated", GlobalVariables.duration_dialog));
        battleSystem.endBattle(true);
    }
}
