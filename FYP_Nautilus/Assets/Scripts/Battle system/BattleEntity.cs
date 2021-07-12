using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleEntity : MonoBehaviour, I_OnRoundStart
{
    public BattleSystem battleSystem;
    public Slider HPbar;

    public string entityName;
    public Sprite pose;

    public int HP;
    public int max_HP;
    public int PP;
    public int PD;
    public int AP;
    public int AD;
    public int Spd;

    public int PP_buff;
    public int PD_buff;
    public int AP_buff;
    public int AD_buff;
    public int Spd_buff;

    public int finalPP { get { return Mathf.CeilToInt(PP * (1 + 0.1f * PP_buff)); } }
    public int finalPD { get { return Mathf.CeilToInt(PD * (1 + 0.1f * PD_buff)); } }
    public int finalAP { get { return Mathf.CeilToInt(AP * (1 + 0.1f * AP_buff)); } }
    public int finalAD { get { return Mathf.CeilToInt(AD * (1 + 0.1f * AD_buff)); } }
    public int finalSpd { get { return Mathf.CeilToInt(Spd * (1 + 0.1f * Spd_buff)); } }

    public int evadeRate;
    public int extraEvadeRate;
    public int finalEvadeRate { get { return Mathf.Clamp(evadeRate + extraEvadeRate, 0, 100); } }    

    public int index; //-1 is ally bench char, -2 is boss enemy

    public void onRoundStart()
    {
        //round start init
    }

    public virtual IEnumerator action(Action action)
    {
        return null;
    }

    public IEnumerator moveTo(Vector3 goal)
    {
        while (Vector3.Distance(transform.position, goal) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal, Time.deltaTime * Spd / 10);
            yield return null;
        }
    }

    public virtual IEnumerator defeat()
    {
        yield return StartCoroutine(GlobalMethods.printDialog(battleSystem.dialog, "it is defeated", 1.5f));        
        if (index == -2)
        {
            battleSystem.endBattle(true);
        }
        else
        {
            for (int i = 0; i < battleSystem.actions.Count; i++)
            {
                if (battleSystem.actions[i].user == this)
                {
                    Action newAction = battleSystem.actions[i];
                    newAction.cancelled = true;
                    battleSystem.actions[i] = newAction;
                }
            }
        }
        
        
    }
}
