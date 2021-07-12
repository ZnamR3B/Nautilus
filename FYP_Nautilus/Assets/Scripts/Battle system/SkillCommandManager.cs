using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkillCommandManager : MonoBehaviour
{
    public BattleSystem battleSystem;
    public GameObject skillButtonPrefab;
    public Transform skillButtonHolder;
    public void openPanel()
    {
        //reset skill list
        foreach(Transform obj in skillButtonHolder.transform)
        {
            Destroy(obj.gameObject);
        }
        //add skills into list
        foreach(Skill skill in battleSystem.allyChar[battleSystem.currentCharIndex].skills)
        {
            SkillButton script =
                Instantiate(skillButtonPrefab, skillButtonHolder).GetComponent<SkillButton>();
            script.skill = skill;
            script.battleSystem = battleSystem;
            script.GetComponentInChildren<TextMeshProUGUI>().text = skill.skillName;
        }
    }
}
