using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WeaponMenuManager : MonoBehaviour, I_ShowSkillInfo
{
    public MenuManager menuManager;
    public WeaponManager weaponManager;
    public TeamMenuManager teamMenuManager;

    public TextMeshProUGUI skillInfo;

    public Transform weaponPanelHolder;
    public GameObject weaponPanelPrefab;

    public GameObject skillInfoPanelPrefab;

    public Sprite[] weaponTypeIcons;
    public Sprite[] skillElementIcons;
    public Sprite[] skillTypeIcons;

    public Image[] left_right_arrows; // 0 = left , 1 = right

    public bool inMenu;
    public bool queryFromTeamMenu;

    public float pageSlideValue;
    float maxSliderValue;
    [SerializeField]
    float sliderSpeed;
    bool showingSkillInfo;

    public int returnWeaponIndex;

    private void Update()
    {
        if(inMenu)
        {
            pageSlideValue -= Input.GetAxisRaw("Horizontal") * (sliderSpeed * Time.deltaTime);
            if (pageSlideValue >= 0)
            {
                pageSlideValue = 0;
                left_right_arrows[0].gameObject.SetActive(false);
            }
            else if (pageSlideValue <= maxSliderValue)
            {
                pageSlideValue = maxSliderValue;
                left_right_arrows[1].gameObject.SetActive(false);
            }
            else
            {
                left_right_arrows[0].gameObject.SetActive(true);
                left_right_arrows[1].gameObject.SetActive(true);
            }
            weaponPanelHolder.transform.DOMoveX(pageSlideValue + 90, 0.01f);            
            if (Input.GetButtonDown("Cancel"))
            {
                if(!queryFromTeamMenu)
                {
                    if (showingSkillInfo)
                        closeSkillInfo();
                    else
                        closeMenu();
                }
                else
                {
                    queryCloseMenu();
                }
            }
        }
    }
    public void openMenu()
    {
        inMenu = true;
        //load weapons 

        //spawn weapon panel list
        int iterator = 0;
        foreach(Weapon weapon in weaponManager.weaponInventory)
        {
            GameObject thisPanel =
                Instantiate(weaponPanelPrefab, weaponPanelHolder);
            //init script on button
            if(queryFromTeamMenu)
                thisPanel.GetComponent<WeaponButton>().initButton(iterator, this);
            thisPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapon.weaponName;
            thisPanel.transform.GetChild(1).GetComponent<Image>().sprite = weaponTypeIcons[(int)weapon.type];
            thisPanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = 
                "HP: " + weapon.extraHP + "\tSpd: " + weapon.extraSpd + 
                "\nPP: " + weapon.extraPP + "\tPD: " + weapon.extraPD + 
                "\nAP: " + weapon.extraAP + "\tAD: " + weapon.extraAD;
            foreach(Skill skill in weapon.skills)
            {
                GameObject skillPanel =
                    Instantiate(skillInfoPanelPrefab, thisPanel.transform.GetChild(4));
                skillPanel.transform.GetChild(0).GetComponent<Image>().sprite = skillElementIcons[(int)skill.skillElement];
                skillPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = skill.skillName;
                skillPanel.transform.GetChild(2).GetComponent<Image>().sprite = skillTypeIcons[(int)skill.type];
                skillPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = skill.power.ToString();
                skillPanel.GetComponent<MenuSkillButton>().initPanel(this, skill);
            }
            //extra stats info
            //HP: xxx   Spd: xxx
            //PP: xxx   PD: xxx
            //AP: xxx   AD: xxx
            iterator++;
        }
        maxSliderValue = (weaponPanelHolder.childCount - 3.5f) * -495;
        //set weapon panel's user
        iterator = 0;
        foreach(Character ch in menuManager.playerCharacterManager.characters)
        {
            int index = ch.weaponIndex;
            if(index >= 0)
            {
                weaponPanelHolder.GetChild(index).GetChild(3).gameObject.SetActive(true);
                weaponPanelHolder.GetChild(index).GetChild(3).GetChild(0).GetComponent<Image>().sprite = ch.icon;
                weaponPanelHolder.GetChild(index).GetComponent<WeaponButton>().userIndex = iterator;
                iterator++;
            }
        }
    }

    public void queryOpenMenu(int i)
    {
        queryFromTeamMenu = true;
        returnWeaponIndex = i;
        openMenu();        
    }
    void closeMenu()
    {
        //clear result before
        foreach (Transform panel in weaponPanelHolder)
        {
            Destroy(panel.gameObject);
        }
        pageSlideValue = 0;
        inMenu = false;
        gameObject.SetActive(false);
        menuManager.openMenu();
    }
    public void queryCloseMenu()
    {
        //clear result before
        foreach (Transform panel in weaponPanelHolder)
        {
            Destroy(panel.gameObject);
        }
        queryFromTeamMenu = false;
        inMenu = false;
        gameObject.SetActive(false);
        teamMenuManager.gameObject.SetActive(true);
        teamMenuManager.returnWeaponQuery(returnWeaponIndex); 
    }

    public void showSkillInfo(Skill skill)
    {
        skillInfo.transform.parent.DOMoveX(0, 0.15f);
        skillInfo.text = skill.skillName + "\nPower: " + skill.power + "\tAccuracy: " + skill.accuracy + "\tMove: (" + skill.moveZ + "," + skill.moveY + "\nRange: " + skill.range + "\tO2: " + skill.o2 + "\n" + skill.description;
    }

    public void closeSkillInfo()
    {
        skillInfo.transform.parent.DOMoveX(800, 0.15f);
    }

    //public IEnumerator showCaution_weaponIsUsed()
    //{
    //    bool result = false;
    //    bool confirmed = false;
    //    while(!confirmed)
    //    {
    //        yield return null;
    //    }
    //    if(confirmed && result)
    //    {

    //    }
    //}
}
