using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite icon;
    public string itemName;
    public int soldPrice;

    public bool usable_field;
    public bool usable_battle;

    public string description;
    public virtual IEnumerator onUseInBattle(BattleEntity target)
    {
        return null;
    }

    public virtual IEnumerator onUseInField()
    {
        return null;
    }
}
