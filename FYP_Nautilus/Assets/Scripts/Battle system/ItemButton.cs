using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item thisItem;
    public ItemPanelManager manager;

    public void initButton(Item i, ItemPanelManager m)
    {
        thisItem = i;
        manager = m;
        GetComponent<Button>().onClick.AddListener(delegate { addItem(); });
    }
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
        I_OnActionAdded[] scripts = manager.battleSystem.allyChar[manager.battleSystem.currentCharIndex].GetComponents<I_OnActionAdded>();
        foreach (I_OnActionAdded s in scripts)
        {
            s.onActionAdded(action);
        }
        manager.battleSystem.nextChar();
    }
}
