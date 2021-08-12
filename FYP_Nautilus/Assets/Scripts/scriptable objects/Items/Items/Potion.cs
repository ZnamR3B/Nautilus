﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[CreateAssetMenu(fileName = "Potion", menuName = "Items/Potions", order = 1)]
public class Potion : Item
{
    public int recoverHP;
    public override IEnumerator onUseInBattle(BattleEntity target)
    {
        yield return GlobalCoroutiner.instance.StartCoroutine(GlobalMethods.printDialog(target.battleSystem.dialog, target.entityName + " used " + itemName, GlobalVariables.duration_dialog));
        yield return GlobalCoroutiner.instance.StartCoroutine(target.recover(recoverHP));
    }
}
