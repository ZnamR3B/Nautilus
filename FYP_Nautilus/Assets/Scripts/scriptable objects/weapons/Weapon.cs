using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Sword, Axe, Twins, Cannon};
[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/basicWeapons", order = 1)]
public class Weapon : ScriptableObject
{
    public string weaponName;
    //extra stats
    public int extraHP;
    public int extraPP;
    public int extraPD;
    public int extraAP;
    public int extraAD;
    public int extraSpd;
    public int extraO2;

    //basic info
    public WeaponType type;

    public Skill basicSkill;

    public Skill[] skills;
}
