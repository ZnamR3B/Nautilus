using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Destination : Quest
{
    public int targetDestinationIndex;

    public override string objectiveProgress()
    {
        return "0 / 1";
    }
}
