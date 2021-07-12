using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Passive_Twins : MonoBehaviour, I_OnAfterAttack
{

    public void onAfterAttack(BattleEntity target, Skill skill)
    {
        extraDamage(target);
    }

    public IEnumerator extraDamage(BattleEntity target)
    {
        if(Random.Range(0,100) < 30)
        {
            target.HP -= 50; //constant damage
            yield return StartCoroutine(GlobalMethods.printDialog(target.battleSystem.dialog, "extra hit!", GlobalVariables.duration_dialog));
            target.HPbar.DOValue((float)target.HP / target.max_HP, 1f);
            yield return new WaitForSeconds(GlobalVariables.duration_HPReduce);
        }
    }
}
