using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMonster : MonoBehaviour
{
    public string monsterName;
    public GameObject battleAvatar;

    public int minDist;
    public int maxDist;

    public int fieldType;

    //public reference:
    public BattleSystem battleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //triggeres
            battleSystem.gameObject.SetActive(true);
            StartCoroutine(battleSystem.battleTriggered(this));
        }
    }
}
