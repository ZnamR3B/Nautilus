using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SkillType {physical, arts, effect};
public enum Element { none, flare, thunder, wind, earth};

public enum SkillTargetType { single, all, other};

[CreateAssetMenu(fileName = "Skill", menuName = "Skills/basicSkill", order = 1)]
public class Skill : ScriptableObject
{
    public string skillName;
    public Element skillElement;
    public int prio;
    public int range;
    public int power;
    public int accuracy; // 999 to be must-hit
    public SkillType type;

    public int moveZ;
    public int moveY;

    public int o2;
    public int hate;
    public string description;
    public SkillTargetType targetType;
    public float checkElementMultiply(Element element, Enemy target)
    {
        if (skillElement == Element.flare)
        {
                //skill is flare
                if (target.element == Element.wind)
                {
                    //not effective
                    return 0.5f;
                }
                if (target.element == Element.thunder)
                {
                    //effective
                    return 1.5f;
                }
        }
        else if (skillElement == Element.earth)
        {
            //skill is earth
            if (target.element == Element.thunder)
            {
                //not effective
                return 0.5f;
            }
            if (target.element == Element.wind)
            {
                //effective
                return 1.5f;
            }
        }
        else if (skillElement == Element.thunder)
        {
            //skill is thunder
            if (target.element == Element.flare)
            {
                //not effective
                return 0.5f;
            }
            if (target.element == Element.earth)
            {
                //effective
                return 1.5f;
            }
        }
        else if (skillElement == Element.wind)
        {
            //skill is wind
            if (target.element == Element.earth)
            {
                //not effective
                return 0.5f;
            }
            if (target.element == Element.flare)
            {
                //effective
                return 1.5f;
            }
        }
        return 1;
    }



    public IEnumerator use(AllyCharacter user, Enemy target)
    {
        if (accuracy == 999 || Random.Range(0, 100) > accuracy - target.finalEvadeRate)
        {
            yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "it hits!", 1.5f));
            //hit
            int dmg = 0;
            if (type == SkillType.physical)
            {
                dmg = user.finalPP - target.finalPD;

            }
            else if (type == SkillType.arts)
            {
                dmg = user.finalAP - target.finalAD;
            }
            else
            {
                //effect no dmg
                yield return null;
            }
            dmg += power;
            float elementEffect = checkElementMultiply(skillElement, target);

            dmg = Mathf.CeilToInt(dmg * elementEffect);
            if (dmg <= 0)
            {
                dmg = 1;
            }
            //yield return 
            target.HP -= dmg;
            target.HPbar.DOValue((float)target.HP / target.max_HP, 1.5f);
            yield return new WaitForSeconds(1.5f);
            user.hate += hate;
            if (target.HP <= 0)
            {
                target.defeat();
            }
        }
        else
        {
            //missed
            yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "it missed...", 1.5f));
        }
        yield return null;
    }

    public IEnumerator use(AllyCharacter user, BattleEntity[] targets)
    {
        foreach (BattleEntity target in targets)
        {
            if (target.GetComponent<Enemy>() != null)
            {
                use(user, target.GetComponent<Enemy>());
            }
            else
            {
                if (accuracy == 999 || Random.Range(0, 100) > accuracy - target.finalEvadeRate)
                {
                    //GlobalMethods.printDialog(user.battleSystem.dialog, "it hits!", 1.5f);
                    //hit
                    int dmg = 0;
                    if(type == SkillType.effect)
                    {
                        //effect no dmg
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
                        user.hate += hate;
                        yield return new WaitForSeconds(1.5f);
                        if (target.HP <= 0)
                        {
                            target.defeat();
                        }
                    }
                    

                }
                else
                {
                    //missed
                    yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(user.battleSystem.dialog, "it missed...", 1.5f));
                }
                yield return null;
            }
        }
    }

    public virtual IEnumerator use(Enemy user)
    {
        yield return null;
    }
}
