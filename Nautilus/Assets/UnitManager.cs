using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //singleton
    private static UnitManager _instance;
    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    //unit list
    //public BattleUnit[] units;
    public int[] unitCount;
    public GameObject[] unitObjects;
}
