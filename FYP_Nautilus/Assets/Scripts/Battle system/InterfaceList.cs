using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_OnRoundStart
{
    void onRoundStart();
}
public interface I_OnRoundEnd
{
    void onRoundEnd();
}
public interface I_OnActionAdded
{
    void onActionAdded(Action action);
}

public interface I_OnBeforeAttack
{
    void onBeforeAttack(Skill skill);
}

public interface I_OnAfterAttack
{
    void onAfterAttack(BattleEntity target, Skill skill);
}

public interface I_OnBeforeAction
{
    void onBeforeAction(BattleEntity user);
}


public interface I_OnAfterAction
{
    void onAtferAction(BattleEntity user);
}

