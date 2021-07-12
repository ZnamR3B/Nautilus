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
            if(range + user.extraRange >= battleSystem.distCount - target.index % user.battleSystem.distCount)
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
        onBeforeAttack(user);
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
            if (range + user.extraRange >=  user.battleSystem.distCount - target.index % user.battleSystem.distCount )
            {
                Debug.Log("original target is in range");
                //if in range
                yield return GlobalCoroutiner.instance.StartCoroutine(dmg(user, target));
            }
            else
            {
                Debug.Log("original target is not in range");
                // this will have the following problem:
                //if before characters move and it decide its action, it will have chance that character is out-of-ranged which originally dont

                ////yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "But not in-range", GlobalVariables.duration_dialog));

                //here is the alternative method to solve the problem stated:
                //re-target if not in range for the original target.
                //but it will make game more difficult(?)
                //target the in-range char who have highest hate

                int maxHate = -1;
                target = null;
                foreach (AllyCharacter ch in user.battleSystem.allyChar)
                {
                    if (range + user.extraRange >= user.battleSystem.distCount - ch.index % user.battleSystem.distCount && ch.hate >= maxHate)
                    {
                        if(maxHate == ch.hate && target != null)
                        {
                            if(Random.Range(0,2)%2 == 0)
                            {
                                maxHate = ch.hate;
                                target = ch;
                            }                            
                        }
                        else
                        {
                            maxHate = ch.hate;
                            target = ch;
                        }
                    }
                }
                Debug.Log("new target is: " + target);
                if (target != null)
                {
                    yield return GlobalCoroutiner.instance.StartCoroutine(dmg(user,target));
                }
                else
                {
                    yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "But not in-range", GlobalVariables.duration_dialog));
                }
            }
            
            
        }
        else if(enemySkillTargetType == enemySkillTarget.range)
        {
            foreach(AllyCharacter target in user.battleSystem.allyChar)
            {
                if(range + user.extraRange >= user.battleSystem.distCount - target.index % user.battleSystem.distCount)
                {
                    yield return GlobalCoroutiner.instance.StartCoroutine(dmg(user, target));
                }
                else
                {
                    yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "But not in-range", GlobalVariables.duration_dialog));
                }
            }
        }
        
    }

    public IEnumerator dmg(Enemy user, AllyCharacter target)
    {
        int dmg = extraPower;
        if(Random.Range(0,100) < accuracy + extraAcc - target.finalEvadeRate)
        {
            //hit
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
                target.HPbar.DOValue((float)target.HP / target.max_HP, GlobalVariables.duration_HPReduce);                
                yield return new WaitForSeconds(GlobalVariables.duration_HPReduce);
                target.hate -= angerReduce;
                if (target.hate <= 0)
                {
                    target.hate = 0;
                }
                if (target.HP <= 0)
                {
                    yield return GlobalCoroutiner.instance.StartCoroutine(target.defeat());
                }
            }
        }
        else
        {
            yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "But it missed...", GlobalVariables.duration_dialog));
        }
    }
}
