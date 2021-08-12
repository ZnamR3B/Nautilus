using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    int weaponIndex;
    public int userIndex;
    WeaponMenuManager manager;
    public void initButton(int i, WeaponMenuManager menu)
    {
        manager = menu;
        weaponIndex = i;
        userIndex = -1;
        GetComponent<Button>().onClick.AddListener(delegate {returnResult(); });
    }

    public void returnResult()
    {
        manager.returnWeaponIndex = weaponIndex;
        if(userIndex != -1)
        {
            //there is a char using this weapon
            //remove that char equipping weapon 
            manager.menuManager.playerCharacterManager.characters[userIndex].weaponIndex = -1; //-1 is null for equipped weapon
        }
        manager.queryCloseMenu();
    }
}
