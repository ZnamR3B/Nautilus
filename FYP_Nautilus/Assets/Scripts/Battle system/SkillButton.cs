using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public BattleSystem battleSystem;
    public Skill skill;
    Button btn;
    
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate { addAction(); });
    }

    public void addAction()
    {
        Action action = new Action();
        action.skill = skill;
        action.user = battleSystem.allyChar[battleSystem.currentCharIndex];
        action.prio = skill.prio;
        action.cancelled = false;
        battleSystem.actions.Add(action);
        I_OnActionAdded[] scripts = action.user.GetComponents<I_OnActionAdded>();
        foreach(I_OnActionAdded s in scripts)
        {
            s.onActionAdded(action);
        }
        battleSystem.nextChar();
    }
}
