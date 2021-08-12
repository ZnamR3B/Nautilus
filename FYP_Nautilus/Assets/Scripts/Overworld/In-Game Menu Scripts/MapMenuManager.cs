using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuManager : MonoBehaviour
{
    //reference
    public MenuManager menuManager;
    public TransferManager transferManager;
    public PlayerInfo playerInfo;
    //holder
    public Transform areaButtonsHolder;
    public Transform areaMapHolder;

    //UI element

    bool inMenu;
    public void openMenu()
    {
        inMenu = true;
        for(int i = 0; i < playerInfo.areas.Length; i++)
        {            
            if (playerInfo.areas[i].unlocked)
            {
                //area is unlocked -> enable the button
                GameObject button = areaButtonsHolder.GetChild(i).gameObject;
                button.SetActive(true);
                button.GetComponent<MapMenuAreaButton>().initButton(this, i);
            }
        }
    }

    public void openArea(int mapIndex)
    {
        areaMapHolder.GetChild(mapIndex).gameObject.SetActive(true);
        for(int i = 0; i < playerInfo.areas[mapIndex].areaPoints.Length; i++)
        {
            if(playerInfo.areas[mapIndex].areaPoints[i].unlocked)
            {
                GameObject button = areaMapHolder.GetChild(mapIndex).GetChild(0).GetChild(i).gameObject;                
                button.SetActive(true);
                button.GetComponent<MapMenuPointButton>().initButton(transferManager, menuManager, this, playerInfo.areas[mapIndex].areaPoints[i]);
            }
        }
    }
    public void closeMenu()
    {
        inMenu = false;
        //set map object inactive
        foreach(Transform map in areaMapHolder)
        {
            map.gameObject.SetActive(false);
        }
        //clsoe menu
        gameObject.SetActive(false);
        menuManager.openMenu();
    }

    
}
