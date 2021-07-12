using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Axe : MonoBehaviour, I_OnAfterAttack
{
    public void onAfterAttack(BattleEntity target, Skill skill)
    {
        StartCoroutine(faintTarget(target)); ;
    }

    public IEnumerator faintTarget(BattleEntity target)
    {
        if (Random.Range(0, 100) < 10) //with 10%
        {
            //faint target
            yield return StartCoroutine(GlobalMethods.printDialog(target.battleSystem.dialog, "Fainted the target!", GlobalVariables.duration_dialog));
        }
    }
}
