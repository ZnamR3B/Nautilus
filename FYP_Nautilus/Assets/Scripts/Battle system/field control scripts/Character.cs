using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int id;
    public string characterName;

    //public Material skin;
    public Sprite icon;
    public Sprite pose;

    public GameObject battleAvatar;

    public Color charUIColor;

    //basic
    public int remainHP;
    public int remainO2;
    public int base_O2;
    public int base_HP;
    public int base_PP;
    public int base_PD;
    public int base_AP;
    public int base_AD;
    public int base_Spd;

    public int weaponIndex;
    public Weapon equippedWeapon 
    { 
        get { return weaponIndex >= 0 ? GetComponentInParent<WeaponManager>().weaponInventory[weaponIndex] : null; }        
    }

    public Skill[] diverSkills;

}
