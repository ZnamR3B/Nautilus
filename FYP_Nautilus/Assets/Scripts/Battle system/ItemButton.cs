using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public Item thisItem;
    public ItemPanelManager manager;
    public void addItem()
    {
        Action action;
        action.item = thisItem;
        action.ch = null;
        action.cancelled = false;
        action.prio = 0;
        action.skill = null;
        action.user = manager.battleSystem.allyChar[manager.battleSystem.currentCharIndex];
        manager.battleSystem.actions.Add(action);
        manager.battleSystem.nextChar();
    }
}
