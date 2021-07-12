using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AllyCharacter : BattleEntity
{
    public int O2;
    public int maxO2;
    public Slider O2bar;
    public bool onField;
    public Skill[] skills;
    public int hate;
    public int heightIndex; // 0 is init height level // +1 = 1 level higher
    public int laneIndex { get { return (index != -1) ? index / battleSystem.distCount : -1; } }
    public void initEntity(Character ch, BattleSystem bs, int lane, bool field)
    {
        //init basic stats
        entityName = ch.characterName;
        HP = ch.remainHP + ch.equippedWeapon.extraHP;
        PP = ch.base_PP + ch.equippedWeapon.extraPP;
        PD = ch.base_PD + ch.equippedWeapon.extraPD;
        AP = ch.base_AP + ch.equippedWeapon.extraAP;
        AD = ch.base_AD + ch.equippedWeapon.extraAD;
        Spd = ch.base_Spd + ch.equippedWeapon.extraSpd;
        max_HP = HP;
        O2 = ch.remainO2 + ch.equippedWeapon.extraO2;
        maxO2 = ch.base_O2 + ch.equippedWeapon.extraO2;
        hate = 0;
        //init skills
        skills = new Skill[ch.equippedWeapon.skills.Length + 1];
        skills[0] = ch.equippedWeapon.basicSkill;
        if(ch.equippedWeapon.skills.Length > 0)
        {
            for (int i = 0; i < ch.equippedWeapon.skills.Length; i++)
            {
                skills[i + 1] = ch.equippedWeapon.skills[i];
            }
        }
        //referencing
        battleSystem = bs;
        //field info
        index = lane >= 0 ? lane * battleSystem.distCount : -1;
        heightIndex = 0;
        onField = field;        
        pose = ch.pose;
    }

    public override IEnumerator action(Action action)
    {
        if(action.skill != null)
        {            
            Skill skill = action.skill;
            //check dist between on-lane enemy or boss enemy
            int moveZ = skill.moveZ;
            if (skill.moveZ > battleSystem.distCount - index % battleSystem.distCount - 1)
            {
                //need return back            
                int remainDist = skill.moveZ - (battleSystem.distCount - index % battleSystem.distCount - 1);
                moveZ -= remainDist;
            }
            //move vertical and move forward
            if (skill.moveY + heightIndex > battleSystem.maxHeight)
            {
                heightIndex = battleSystem.maxHeight;
            }
            else
            {
                heightIndex += skill.moveY;
            }
            if (skill.moveY + heightIndex < battleSystem.minHeight)
            {
                heightIndex = battleSystem.minHeight;
            }
            else
            {
                heightIndex += skill.moveY;
            }

            //move
            yield return StartCoroutine(moveTo(new Vector3(transform.position.x, transform.parent.position.y + heightIndex * 2, transform.position.z)));
            yield return new WaitForSeconds(0.3f);
            index += moveZ;
            yield return StartCoroutine(moveTo(battleSystem.fieldUnits[index].transform.position + new Vector3(0, 1, 0)));
            yield return new WaitForSeconds(0.5f);
            //if moved but not in range, end action
            //else attack by skill. Deal damage and do skill effects
            if (skill.range >= battleSystem.distCount - index % battleSystem.distCount && skill.targetType == SkillTargetType.single)
            {
                //in-range
                skill.use(this, battleSystem.enemy);
            }
            else if (skill.targetType == SkillTargetType.all)
            {
                List<BattleEntity> targets = null;
                foreach (BattleEntity e in battleSystem.allBattleUnits)
                {
                    targets.Add(e);
                }
                skill.use(this, targets.ToArray());
            }
            else if (skill.targetType == SkillTargetType.other)
            {
                skill.use(this, skillTarget());
            }
            else
            {
                //not in range
                yield return GlobalMethods.printDialog(battleSystem.dialog, "but not in-range...", 1f);
            }
            //do end action
        }
    }

    public virtual BattleEntity[] skillTarget()
    {
        return null;
    }

    public override IEnumerator defeat()
    {
        hate = 0;
        return base.defeat();
    }


}
