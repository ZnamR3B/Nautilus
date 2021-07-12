using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string itemName;
    public int soldPrice;

    public bool usable_field;
    public bool usable_battle;

    public virtual void onUseInBattle()
    {

    }

    public virtual void onUseInField()
    {

    }
}
