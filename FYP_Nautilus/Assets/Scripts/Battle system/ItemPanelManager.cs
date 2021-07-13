using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
