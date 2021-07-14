using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class AllyCharacter : BattleEntity
{
    public Character info;
    public bool alive;
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
        alive = true;
        //init skills
        skills = new Skill[ch.equippedWeapon.skills.Length + 1];
        skills[0] = ch.equippedWeapon.basicSkill;
        canMove = true;
        if(ch.equippedWeapon.skills.Length > 0)
        {
            for (int i = 0; i < ch.equippedWeapon.skills.Length; i++)
            {
                skills[i + 1] = ch.equippedWeapon.skills[i];
            }
        }
        //give passive skill
        switch(ch.equippedWeapon.type)
        {
            case WeaponType.Sword:
                gameObject.AddComponent<Passive_Sword>();
                break;
            case WeaponType.Axe:
                gameObject.AddComponent<Passive_Axe>();
                break;
            case WeaponType.Cannon:
                gameObject.AddComponent<Passive_Cannon>();
                break;
            case WeaponType.Twins:
                gameObject.AddComponent<Passive_Twins>();
                break;
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
        //trigger onBeforeAction action
        onBeforeAction(this);
        if(action.skill != null && canMove)
        {            
            Skill skill = action.skill;
            //check dist between on-lane enemy or boss enemy
            int moveZ = skill.moveZ;
            int remainDist = 0;
            if (skill.moveZ > battleSystem.distCount - index % battleSystem.distCount - 1)
            {
                //need return back            
                remainDist = skill.moveZ - (battleSystem.distCount - index % battleSystem.distCount - 1);
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
            //reduce o2
            if(O2 - skill.o2 < 0)
            {
                //cant use
                yield return StartCoroutine(GlobalMethods.printDialog(battleSystem.dialog, "Not enough Oxygen to move...!", GlobalVariables.duration_dialog));
            }
            else
            {
                O2bar.value = (float) O2 / maxO2;
                yield return new WaitForSeconds(GlobalVariables.duration_HPReduce);
                //move
                yield return StartCoroutine(moveTo(new Vector3(transform.position.x, transform.parent.position.y + heightIndex * 2, transform.position.z)));
                yield return new WaitForSeconds(0.3f);
                index += moveZ;
                yield return StartCoroutine(moveTo(battleSystem.fieldUnits[index].transform.position + new Vector3(0, 1, 0)));
                yield return new WaitForSeconds(0.5f);
                //if moved but not in range, end action
                //else attack by skill. Deal damage and do skill effects
                if (skill.range + extraRange >= battleSystem.distCount - index % battleSystem.distCount && skill.targetType == SkillTargetType.single)
                {
                    //in-range
                    yield return StartCoroutine(skill.use(this, battleSystem.enemy));
                }
                else if (skill.targetType == SkillTargetType.all)
                {
                    List<BattleEntity> targets = null;
                    foreach (BattleEntity e in battleSystem.allBattleUnits)
                    {
                        targets.Add(e);
                    }
                    yield return StartCoroutine(skill.use(this, targets.ToArray()));
                }
                else if (skill.targetType == SkillTargetType.other)
                {
                    yield return StartCoroutine(skill.use(this, skillTarget()));
                }
                else
                {
                    //not in range
                    yield return GlobalMethods.printDialog(battleSystem.dialog, "but not in-range...", GlobalVariables.duration_dialog);
                }
                //do end action
            }
            yield return new WaitForSeconds(.25f);
            if(remainDist > 0)
            {
                if(remainDist > index % battleSystem.distCount )
                {
                    index = laneIndex * battleSystem.distCount;
                }
                else
                {
                    index -= remainDist;
                }
                yield return StartCoroutine(moveTo(battleSystem.fieldUnits[index].transform.position + new Vector3(0, 1, 0)));

            }
            onAfterAction(this);
        }
        else
        {
            yield return StartCoroutine(GlobalMethods.printDialog(battleSystem.dialog, " but cannot move...", GlobalVariables.duration_dialog));
        }
    }

    public virtual BattleEntity[] skillTarget()
    {
        return null;
    }

    public override IEnumerator defeat()
    {
        hate = 0;
        alive = false;
        index = -1;
        onField = false;
        HPbar = null;
        O2bar = null;
        
        Destroy(battleSystem.charInfoPanelHolder.GetChild(System.Array.IndexOf(battleSystem.allyChar, this)).gameObject);
        yield return StartCoroutine(moveTo(battleSystem.characterHolder.position + new Vector3(0, 0, -10)));
        base.defeat();
    }

    public IEnumerator switchWith(AllyCharacter ch)
    {
        //swap index
        int tempIndex = ch.index;
        ch.index = index;
        index = tempIndex;
        //get its array index of script in battle system's allyChar[]
        int allyCharIndex_user = System.Array.IndexOf(battleSystem.allyChar, this);
        int allyCharIndex_target = System.Array.IndexOf(battleSystem.allyChar, ch);
        //set info panel
        if (ch.onField && onField) //both onField
        {            
            battleSystem.charInfoPanelHolder.GetChild(allyCharIndex_user).SetSiblingIndex(allyCharIndex_target);
            battleSystem.charInfoPanelHolder.GetChild(allyCharIndex_target).SetSiblingIndex(allyCharIndex_user);
        }
        else
        {
            //only this character on field
            Destroy(battleSystem.charInfoPanelHolder.GetChild(allyCharIndex_user).gameObject);
            battleSystem.addCharInfoPanel(battleSystem.allyChar[allyCharIndex_target], laneIndex);
        }
        //swap onField boolean value
        bool tempBool = onField;
        onField = ch.onField;
        ch.onField = tempBool;

        //swap script position in array
        AllyCharacter temp = battleSystem.allyChar[allyCharIndex_user];
        battleSystem.allyChar[allyCharIndex_user] = battleSystem.allyChar[allyCharIndex_target];
        battleSystem.allyChar[allyCharIndex_target] = temp;
        //swap position
        StartCoroutine(ch.moveTo(battleSystem.fieldUnits[ch.index].transform.position + new Vector3(0, 1, 0)));
        StartCoroutine(moveTo(battleSystem.fieldUnits[index].transform.position + new Vector3(0, 1, 0)));
        yield return new WaitForSeconds(.5f);
    }

}
