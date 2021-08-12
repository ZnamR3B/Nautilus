using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemMenuIcon : MonoBehaviour
{
    ItemMenuManager itemMenuManager;
    Item thisItem;

    public void initIcon(ItemMenuManager manager, Item i)
    {
        thisItem = i;
        itemMenuManager = manager;
        GetComponent<Button>().onClick.AddListener(delegate { showInfo(); });
    }
    public void showInfo()
    {
        itemMenuManager.showItemInfo(thisItem);
    }
    
}
