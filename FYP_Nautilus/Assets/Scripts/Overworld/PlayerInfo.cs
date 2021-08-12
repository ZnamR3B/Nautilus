using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapArea
{
    public string areaName;
    public bool unlocked;
    public AreaPoint[] areaPoints;
}

[System.Serializable]
public struct AreaPoint
{
    public string pointName;
    public bool unlocked;
    public int sceneIndex;
    public int placeIndex;
}

[System.Serializable]
public struct OceanDivePoint
{
    public string pointName;
    public bool unlocked;
    public int depth;
    public Vector2Int coordinate;
}

public class PlayerInfo : MonoBehaviour
{
    public int money;

    public int mainQuestCleared;

    public MapArea[] areas;

    public OceanDivePoint[] RondaOceanDivePoints;

    public PlayerCharacterManager playerCharacterManager;
    public ItemManager itemManager;
    public WeaponManager weaponManager;

    public int mapIndex; // 0 = ronda
}
