using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelManager : SubMenuManager
{
    public GameObject itemButtonPrefab;
    public Transform itemButtonHolder;
    public void openPanel()
    {
        foreach (Transform obj in itemButtonHolder)
        {
            Destroy(obj.gameObject);
        }
        for(int i = 0; i < battleSystem.itemManager.items.Length; i++)
        {
            if(battleSystem.itemManager.itemCount[i] > 0)
            {
                //instantiate item
                GameObject icon =
                    Instantiate(itemButtonPrefab, itemButtonHolder);
                icon.GetComponent<ItemButton>().thisItem = battleSystem.itemManager.items[i];
            }
        }
    }
}
