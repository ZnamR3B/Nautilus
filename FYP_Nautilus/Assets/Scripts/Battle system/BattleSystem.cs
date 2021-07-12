﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public struct Action
{
    public int prio;
    public BattleEntity user;
    public Skill skill;
    public bool cancelled;
    public int finalPrio { get {return prio * 1000 + user.finalSpd; } }
}

public class BattleSystem : MonoBehaviour
{
    public GameObject fieldCamera;

    //enemy
    public Enemy enemy;
    public Transform enemyHolder;

    public TextMeshProUGUI enemyName;
    public Slider enemyHPBar;
    //public BattleEntity[] enemyMinions;
    public List<BattleEntity> allBattleUnits = new List<BattleEntity>();

    public Transform characterHolder;
    public AllyCharacter[] allyChar;
    public GameObject[] allyAvatar;

    public PlayerCharacterManager charManager;
    public ItemManager itemManager;

    public int laneCount;
    public int distCount;
    public int roundCount;
    public int minHeight;
    public int maxHeight;

    public GameObject fieldMass;
    public Transform fieldHolder;
    public GameObject[] fieldUnits;

    public Transform backgroundHolder;

    public GameObject[] fieldPrefabs;

    public List<Action> actions = new List<Action>();

    public Transform charInfoPanelHolder;
    public GameObject charInfoPanelPrefab;

    public GameObject mainPanel;
    public GameObject skillPanelObject;
    public GameObject itemPanelObject;
    public GameObject switchPanelObject;
    public Image charPose;

    public GameObject dialog;

    public SkillCommandManager skillManager;
    public ItemPanelManager ItemPanelManager;
    public SwitchManager switchManager;
    //battle input
    public int currentCharIndex;

    public IEnumerator battleTriggered(FieldMonster monster)
    {        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        fieldCamera.SetActive(false);
        //get player characters manager
        charManager = GameObject.FindGameObjectWithTag("PlayerInfo").GetComponent<PlayerCharacterManager>();
        itemManager = charManager.gameObject.GetComponent<ItemManager>();        
        //laneCount & distCount (member in team and dist between enemy
        laneCount = Mathf.Min(3, charManager.teamMembers.Length);
        distCount = Random.Range(monster.minDist, monster.maxDist + 1);
        //generate background field
        enemyHolder.localPosition = new Vector3(0, 4.5f, (distCount - 3) * 3 + 1);
        if (monster.fieldType < fieldPrefabs.Length)
        {
            Instantiate(fieldPrefabs[monster.fieldType], backgroundHolder);
        }
        //spawn enemy
        enemy = Instantiate(monster.battleAvatar, enemyHolder).GetComponent<Enemy>();
        enemy.index = -2;
        enemy.battleSystem = this;
        enemy.HPbar = enemyHPBar;
        enemy.canMove = true;
        allBattleUnits.Add(enemy);        
        
        //init arrays to store battle data
        fieldUnits = new GameObject[laneCount * distCount];
        allyChar = new AllyCharacter[charManager.teamMembers.Length];
        allyAvatar = new GameObject[charManager.teamMembers.Length];
        //generate lane units tile
        for (int l = 0; l < laneCount; l++) 
        {
            int laneX = (l == 0) ? 0 : (l == 1) ? -4 : 4;
            for (int d = 0; d < distCount; d++)
            {
                GameObject tile =
                    Instantiate(fieldMass, fieldHolder.position + new Vector3(laneX , 0, d * 3), Quaternion.Euler(90,0,0), fieldHolder);
                fieldUnits[l * distCount + d] = tile;
            }
            //generate character avatar
            allyAvatar[l] = Instantiate(charManager.characters[charManager.teamMembers[l]].battleAvatar, characterHolder.position + new Vector3(laneX, 0, 0), Quaternion.identity, characterHolder);
            //assign stats to battle entity script
            AllyCharacter script = allyAvatar[l].GetComponent<AllyCharacter>();
            script.initEntity(charManager.characters[charManager.teamMembers[l]], this, l, true);
            allyChar[l] = script;
            //init UI info
            GameObject info =
                Instantiate(charInfoPanelPrefab, charInfoPanelHolder);
            info.GetComponentInChildren<TextMeshProUGUI>().text = script.entityName;
            script.HPbar = info.transform.GetChild(0).GetComponent<Slider>();
            script.HPbar.value = (float)script.HP / script.max_HP;
            script.O2bar = info.transform.GetChild(1).GetComponent<Slider>();
            script.O2bar.value = (float)script.O2 / script.maxO2;
            allBattleUnits.Add(script);
        }
        //generate characters NOT on field initially
        if(charManager.teamMembers.Length > 3)
        {
            for(int i = 3; i< charManager.teamMembers.Length; i++)
            {
                //generate character avatar
                allyAvatar[i] = Instantiate(charManager.characters[charManager.teamMembers[i]].battleAvatar, characterHolder.position + new Vector3(0, 0, -10), Quaternion.identity, characterHolder);
                //assign stats to battle entity script
                AllyCharacter script = allyAvatar[i].GetComponent<AllyCharacter>();
                script.initEntity(charManager.characters[charManager.teamMembers[i]], this, -1, false);
                allyChar[i] = script;
            }
        }
        //init variables
        roundCount = 0;
        yield return StartCoroutine(GlobalMethods.printDialog(dialog, enemy.entityName + " appeared!!!", GlobalVariables.duration_dialog));
        battleRoundStart();
    }

    public void battleRoundStart()
    {
        //round start
        actions.Clear();
        roundCount++;
        currentCharIndex = 0;
        BattleEntity[] entities = allBattleUnits.ToArray();
        sortBattleUnits(entities, 0, entities.Length - 1);
        allBattleUnits.Clear();
        for(int i = 0; i< entities.Length; i++)
        {
            allBattleUnits.Add(entities[i]);
        }
        foreach(BattleEntity entity in allBattleUnits)
        {
            I_OnRoundStart[] scripts = entity.GetComponents<I_OnRoundStart>();
            foreach (I_OnRoundStart s in scripts)
            {
                s.onRoundStart();
            }
        }
        charPose.sprite = allyChar[currentCharIndex].pose;
        mainPanel.SetActive(true);
    }

    void sortBattleUnits(BattleEntity[] arr, int left, int right)
    {
        sortBattleEntities(arr, left, right);
        for (int i = 0; i < arr.Length - 1; i++)
        {            
            if (arr[i].finalSpd == arr[i + 1].finalSpd)
            {
                int last = i + 1;
                while (arr[last].finalSpd == arr[i].finalSpd)
                {
                    if (last >= right)
                    {
                        break;
                    }
                    last++;
                    
                }
                Debug.Log(last);
                BattleEntity[] list = randomSequence(arr, i, last);
                for(int j = 0; j < list.Length; j++)
                {
                    arr[i + j] = list[j];
                }
                i = last;
            }
            
        }
    }
    T[] randomSequence<T>(T[] arr, int left, int right)
    {
        List<T> pool = new List<T>();
        for(int i = left; i <= right; i++)
        {
            pool.Add(arr[i]);
        }
        T[] result = new T[right - left + 1];
        for(int i = 0; i <= right - left ; i++)
        {
            //Debug.Log();
            result[i] = pool[Random.Range(0, pool.Count)];
            pool.Remove(result[i]);
        }
        return result;
    }

    void sortBattleEntities(BattleEntity[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = partitionBattleEntities(arr, left, right);

            if (pivot > 1)
            {
                sortBattleEntities(arr, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                sortBattleEntities(arr, pivot + 1, right);
            }
        }
    }

    int partitionBattleEntities(BattleEntity[] Array, int left, int right)
    {
        int i = left;
        int pivot = Array[right].finalSpd;
        BattleEntity temp;

        for (int j = left; j <= right; j++)
        {
            if (Array[j].finalSpd > pivot)
            {
                temp = Array[i];
                Array[i] = Array[j];
                Array[j] = temp;
                i++;
            }
        }

        temp = Array[right];
        Array[right] = Array[i];
        Array[i] = temp;
        return i;
    }

    public void closeAllPanels()
    {
        mainPanel.SetActive(false);
        itemPanelObject.SetActive(false);
        switchPanelObject.SetActive(false);
        skillPanelObject.SetActive(false);
    }
    public void openSkillPanel()
    {
        mainPanel.SetActive(false);
        itemPanelObject.SetActive(false);
        switchPanelObject.SetActive(false);
        skillPanelObject.SetActive(true);
        //script
        skillManager.openPanel();
    }
    public void openItenPanel()
    {
        mainPanel.SetActive(false);
        itemPanelObject.SetActive(true);
        switchPanelObject.SetActive(false);
        skillPanelObject.SetActive(false);
        //script
        skillManager.openPanel();
    }
    public void openSwitchPanel()
    {
        mainPanel.SetActive(false);
        itemPanelObject.SetActive(false);
        switchPanelObject.SetActive(true);
        skillPanelObject.SetActive(false);
        //script
        skillManager.openPanel();
    }

    public IEnumerator startAction()
    {
        Action[] actionList = actions.ToArray();
        sortBattleAction(actionList, 0, actions.Count - 1);
        actions.Clear();
        foreach(Action action in actionList)
        {
            actions.Add(action);
        }
        foreach(Action action in actions)
        {
            if(!action.cancelled)
            {
                yield return StartCoroutine(GlobalMethods.printDialog(dialog, action.user.entityName + " used " + action.skill.skillName, GlobalVariables.duration_dialog));
                yield return StartCoroutine(action.user.action(action));
                yield return new WaitForSeconds(.25f);
            }
        }
        battleRoundStart();        
    }

    void sortBattleAction(Action[] arr, int left, int right)
    {
        sortActions(arr, left, right);
        //re-arrange entities if same speed. (randomly)
        for (int i = 0; i < arr.Length - 1; i++)
        {
            if (arr[i].finalPrio == arr[i + 1].finalPrio)
            {
                int last = i + 1;
                while (arr[last].finalPrio == arr[i].finalPrio)
                {
                    if (last >= right)
                    {
                        break;
                    }
                    last++;

                }
                Debug.Log(last);
                Action[] list = randomSequence(arr, i, last);
                for (int j = 0; j < list.Length; j++)
                {
                    arr[i + j] = list[j];
                }
                i = last;
            }

        }
    }

    void sortActions(Action[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = partitionActions(arr, left, right);

            if (pivot > 1)
            {
                sortActions(arr, left, pivot - 1);
            }
            if (pivot + 1 < right)
            {
                sortActions(arr, pivot + 1, right);
            }
        }
    }

    int partitionActions(Action[] Array, int left, int right)
    {
        int i = left;
        int pivot = Array[right].finalPrio;
        Action temp;

        for (int j = left; j <= right; j++)
        {
            if (Array[j].finalPrio > pivot)
            {
                temp = Array[i];
                Array[i] = Array[j];
                Array[j] = temp;
                i++;
            }
        }

        temp = Array[right];
        Array[right] = Array[i];
        Array[i] = temp;
        return i;
    }

    public void nextChar()
    {
        if(currentCharIndex == allyChar.Length - 1)
        {
            //end command stage
            //enemy choose action
            enemy.decideAction();
            Debug.Log("enemy decide action");
            closeAllPanels();
            StartCoroutine(startAction());       
        }
        else
        {
            //go next character
            currentCharIndex++;
            //open main panel
            mainPanel.SetActive(true);
            itemPanelObject.SetActive(false);
            switchPanelObject.SetActive(false);
            skillPanelObject.SetActive(false);
            //char sprite
            charPose.sprite = allyChar[currentCharIndex].pose;
        }
    }

    public void lastChar()
    {
        if(currentCharIndex != 0)
        {
            foreach(Action action in actions)
            {
                if(action.user == allyChar[currentCharIndex])
                {
                    actions.Remove(action);
                    break;
                }
            }
            currentCharIndex--;
            mainPanel.SetActive(true);
            itemPanelObject.SetActive(false);
            switchPanelObject.SetActive(false);
            skillPanelObject.SetActive(false);
            //char sprite
            charPose.sprite = allyChar[currentCharIndex].pose;
        }
    }

    public void endBattle(bool win)
    {
        StopAllCoroutines();
        if(win)
        {
            //pass data of HP and o2 left
            for(int i = 0; i < allyChar.Length; i++)
            {
                charManager.characters[charManager.teamMembers[i]].remainHP = allyChar[i].HP;
            }
            //show result

            //return to field
            fieldCamera.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}