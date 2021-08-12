using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSkillButton : MonoBehaviour
{
    Skill skill;
    I_ShowSkillInfo menuManager;

    public void initPanel(I_ShowSkillInfo manager, Skill s)
    {
        skill = s;
        menuManager = manager;
        GetComponent<Button>().onClick.AddListener(delegate { showInfo(); });
    }

    void showInfo()
    {
        menuManager.showSkillInfo(skill);
    }
}
