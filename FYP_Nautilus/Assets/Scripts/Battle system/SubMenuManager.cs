using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuManager : MonoBehaviour
{
    public BattleSystem battleSystem;
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            toMain();
        }
    }

    public void toMain()
    {
        battleSystem.openMainPanel();
    }
}
