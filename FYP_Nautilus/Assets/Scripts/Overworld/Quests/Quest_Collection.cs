using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Collection : Quest
{
    public int targetItemIndex;
    int current
    {
        get
        {
            return FindObjectOfType<ItemManager>().itemCount[targetItemIndex];
        }
    }
    public int goal;

    public override string objectiveProgress()
    {
        return current + " / " + goal;
    }
}
