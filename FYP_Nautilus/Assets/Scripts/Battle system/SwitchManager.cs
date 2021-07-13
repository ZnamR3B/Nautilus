using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitchManager : MonoBehaviour
{
    public BattleSystem battleSystem;
    public AllyCharacter result = null;

    public GameObject charPanelPrefab;

    public Transform charPanelsHolder;
    public void openPanel()
    {
        result = null;
        foreach(AllyCharacter ally in battleSystem.allyChar)
        {
            GameObject panel =
                Instantiate(charPanelPrefab, charPanelsHolder);
            panel.GetComponent<Image>().color = (ally.alive) ? Color.white : Color.black;
            panel.GetComponent<SwitchCharButton>().initButton(ally, this, ally.alive);
            panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ally.entityName;
            panel.transform.GetChild(1).GetComponent<Slider>().value = (float)ally.HP / ally.max_HP;
            panel.transform.GetChild(2).GetComponent<Slider>().value = (float)ally.O2 / ally.maxO2;
            panel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = ally.HP + " / " + ally.max_HP;
            panel.transform.GetChild(4).GetComponent<Image>().sprite = ally.info.icon;
        }
    }
}
