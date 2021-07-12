using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Sword : MonoBehaviour, I_OnBeforeAttack
{
    public void onBeforeAttack(Skill skill)
    {
        skill.extraAcc += 30;
        if(skill.type == SkillType.physical)
            skill.extraPower += 10;
    }

}
