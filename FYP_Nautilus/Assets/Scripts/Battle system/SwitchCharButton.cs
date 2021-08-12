using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCharButton : MonoBehaviour
{
    public AllyCharacter ch;
    public SwitchManager switchManager;
    
    public void initButton(AllyCharacter c, SwitchManager m, bool clickable)
    {
        ch = c;
        switchManager = m;
        if(clickable)
        {
            GetComponent<Button>().onClick.AddListener(delegate { onClicked(); });
        }
    }

    void onClicked()
    {
        switchManager.result = ch;
        if(switchManager.battleSystem.commandState)
        {
            Action action;
            action.user = switchManager.battleSystem.allyChar[switchManager.battleSystem.currentCharIndex];
            action.skill = null;
            action.item = null;
            action.prio = 0;
            action.ch = ch;
            action.cancelled = false;
            switchManager.battleSystem.actions.Add(action);
            I_OnActionAdded[] scripts = action.user.GetComponents<I_OnActionAdded>();
            foreach (I_OnActionAdded s in scripts)
            {
                s.onActionAdded(action);
            }
            switchManager.battleSystem.nextChar();
        }
    }
}
