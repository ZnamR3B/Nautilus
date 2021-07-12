using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum enemySkillTarget { single, range, allEnemy, other};
[CreateAssetMenu(fileName = "basic Enemy skill", menuName = "basicEnemySkill/basicSkill", order = 1 )]
public class EnemySkill : Skill
{
    public enemySkillTarget enemySkillTargetType;
    public int angerReduce;
   
   public bool targetInRange(Enemy user, AllyCharacter target, BattleSystem battleSystem)
   {
        if(enemySkillTargetType == enemySkillTarget.single || enemySkillTargetType == enemySkillTarget.range)
        {
            if(range >= battleSystem.distCount - target.index % 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if(enemySkillTargetType == enemySkillTarget.allEnemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override IEnumerator use(Enemy user)
    {
        
        if (enemySkillTargetType == enemySkillTarget.single)
        {
            //find target
            AllyCharacter target = null;
            int laneCount = user.battleSystem.laneCount;
            if (user.anger > 0)
            {
                int random = Random.Range(0, user.anger) + 1;
                for (int i = 0; i < laneCount; i++)
                {
                    if (random <= user.battleSystem.allyChar[i].hate)
                    {
                        target = user.battleSystem.allyChar[i];
                        break;
                    }
                    else
                    {
                        random -= user.battleSystem.allyChar[i].hate;
                    }
                }
            }
            else
            {
                target = user.battleSystem.allyChar[Random.Range(0, user.battleSystem.laneCount)];
            }
            yield return dmg(user, target);
        }
        else if(enemySkillTargetType == enemySkillTarget.range)
        {
            foreach(AllyCharacter target in user.battleSystem.allyChar)
            {
                if(range >= user.battleSystem.distCount - target.index % user.battleSystem.distCount)
                {
                    yield return dmg(user, target);
                }
            }
        }
    }

    public IEnumerator dmg(Enemy user, AllyCharacter target)
    {
        int dmg = 0;
        if (type == SkillType.effect)
        {
            //do effect
        }
        else
        {
            if (type == SkillType.physical)
            {
                dmg = user.finalPP - target.finalPD;

            }
            else if (type == SkillType.arts)
            {
                dmg = user.finalAP - target.finalAD;
            }
            dmg = Mathf.CeilToInt(dmg);
            if (dmg <= 0)
            {
                dmg = 1;
            }
            //yield return 
            target.HP -= dmg;
            target.HPbar.DOValue((float)target.HP / target.max_HP, 1.5f);
            target.hate -= angerReduce;
            if (target.hate <= 0)
            {
                target.hate = 0;
            }
            yield return new WaitForSeconds(1.5f);
            if (target.HP <= 0)
            {
                target.defeat();
            }
        }
    }
}
