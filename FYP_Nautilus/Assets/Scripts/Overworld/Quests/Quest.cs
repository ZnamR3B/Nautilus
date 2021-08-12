using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum QuestState { locked, unlocked, cleared }
public enum QuestType { kill, collection, destination}

public enum QuestCategory { Story, Side}

[System.Serializable]
public struct Reward
{
    public int money;
    public Item[] rewardItem;
    public Weapon[] rewardWeapon;
}
public abstract class Quest : MonoBehaviour
{
    public int questIndex;

    public QuestType questType;
    public QuestCategory questCategory;

    public string questName;
    public string description;
    public string objectiveDescription;
    public string rewardDescription;

    public QuestState state;

    public Reward questReward;
    public virtual string objectiveProgress()
    {
        return null;
    }


}
