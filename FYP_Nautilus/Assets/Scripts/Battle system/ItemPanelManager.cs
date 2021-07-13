using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
                ItemButton script = icon.GetComponent<ItemButton>();
                script.thisItem = battleSystem.itemManager.items[i];
                script.manager = this;
                icon.GetComponentInChildren<TextMeshProUGUI>().text = battleSystem.itemManager.itemCount[i].ToString();
                icon.GetComponent<Image>().sprite = script.thisItem.icon;
            }
        }
    }
}
