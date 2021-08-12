using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public Sprite icon;
    public int id;
    public string itemName;
    public int sellPrice;
    public int buyPrice;
    public bool usable_field;
    public bool toTarget;
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
    public virtual IEnumerator onUseInField(int charIndex)
    {
        return null;
    }
}
