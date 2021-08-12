using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TeamMenuManager : MonoBehaviour, I_ShowSkillInfo
{
    public MenuManager menuManager;
    public PlayerCharacterManager playerCharacterManager;
    public WeaponMenuManager weaponMenuManager;

    //UI element
    //char info
    public Image charImage;
    public TextMeshProUGUI charName;
    public TextMeshProUGUI charHP;
    public TextMeshProUGUI charSpd;
    public TextMeshProUGUI charPP;
    public TextMeshProUGUI charPD;
    public TextMeshProUGUI charAP;
    public TextMeshProUGUI charAD;
    //weapon info
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponHP;
    public TextMeshProUGUI weaponSpd;
    public TextMeshProUGUI weaponPP;
    public TextMeshProUGUI weaponPD;
    public TextMeshProUGUI weaponAP;
    public TextMeshProUGUI weaponAD;
    public Image weaponType;
    public Sprite[] weaponTypeIcons;
    //skill info
    public GameObject skillPanelPrefab;
    public Transform skillHolder;

    public Sprite[] elementIcons; // none , flare , thunder , wind , earth
    public Sprite[] skillTypeIcons; // physical / arts / effect

    //team member
    public Image[] memberIcons;
    public int currentCharIndex;

    public Image currentCharPointer;

    public bool inMenu;

    public RectTransform skillInfoPanel;
    public TextMeshProUGUI skillInfo;

    public void openMenu()
    {
        inMenu = true;
        currentCharIndex = 0;
        for(int i = 0; i < 5; i++)
        {
            if(i < playerCharacterManager.teamMembers.Length)
            {
                memberIcons[i].transform.parent.gameObject.SetActive(true);
                memberIcons[i].sprite =  playerCharacterManager.characters[playerCharacterManager.teamMembers[i]].icon;
            }
            else
            {
                memberIcons[i].transform.parent.gameObject.SetActive(false);
            }
        }
        loadCharPanel();
    }
    private void Update()
    {
        if(inMenu)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.D))
            {
                currentCharIndex++;
                if(currentCharIndex == playerCharacterManager.teamMembers.Length)
                {
                    currentCharIndex = 0;
                }
                loadCharPanel();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                currentCharIndex--;
                if(currentCharIndex < 0)
                {
                    currentCharIndex = playerCharacterManager.teamMembers.Length - 1;
                }
                loadCharPanel();
            }
            if(Input.GetButtonDown("Cancel"))
            {
                //close menu and return to main menu
                closeMenu();
            }
        }
    }
    public void closeMenu()
    {
        closeSkillInfo();
        inMenu = false;
        gameObject.SetActive(false);
        menuManager.openMenu();
    }
    public void loadCharPanel()
    {
        closeSkillInfo();
        //get current character info
        Character currentChar = playerCharacterManager.characters[playerCharacterManager.teamMembers[currentCharIndex]];
        //set character info
        charImage.sprite = currentChar.pose;
        charName.text = currentChar.characterName;
        charHP.text = "HP: " + currentChar.remainHP + "/" + currentChar.base_HP ;
        charSpd.text = "Spd: " + currentChar.base_Spd;
        charPP.text = "PP: " + currentChar.base_PP;
        charPD.text = "PD: " + currentChar.base_PD;
        charAP.text = "AP: " + currentChar.base_AP;
        charAD.text = "AD: " + currentChar.base_AD;
        //set weapon info
        Weapon weapon = currentChar.equippedWeapon;
        if(weapon != null)
        {
            weaponType.sprite = weaponTypeIcons[(int)weapon.type];
            weaponName.text = weapon.weaponName;
            weaponHP.text = "HP: " + weapon.extraHP;
            weaponSpd.text = "Spd: " + weapon.extraSpd;
            weaponPP.text = "PP: " + weapon.extraPP;
            weaponPD.text = "PD: " + weapon.extraPD;
            weaponAP.text = "AP: " + weapon.extraAP;
            weaponAD.text = "AD: " + weapon.extraAD;
        }
        else
        {
            weaponType.sprite = null;
            weaponName.text = "--unequipped--";
            weaponHP.text = "HP: --";
            weaponSpd.text = "Spd: --";
            weaponPP.text = "PP: --";
            weaponPD.text = "PD: --"; 
            weaponAP.text = "AP: --";
            weaponAD.text = "AD: --";
        }
        //set diver's skill info
        foreach(Transform item in skillHolder)
        {
            Destroy(item.gameObject);
        }
        if(currentChar.diverSkills.Length > 0)
        {
            for (int i = 0; i < currentChar.diverSkills.Length; i++)
            {
                GameObject skillPanel =
                    Instantiate(skillPanelPrefab, skillHolder);
                skillPanel.transform.GetChild(0).GetComponent<Image>().sprite = elementIcons[(int)currentChar.diverSkills[i].skillElement];
                skillPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentChar.diverSkills[i].skillName;
                skillPanel.transform.GetChild(2).GetComponent<Image>().sprite = skillTypeIcons[(int)currentChar.diverSkills[i].type];
                skillPanel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Power: " + currentChar.diverSkills[i].power;
                skillPanel.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Move: " + currentChar.diverSkills[i].moveZ + "," + currentChar.diverSkills[i].moveY;
                skillPanel.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = "Range: " + currentChar.diverSkills[i].range;
                skillPanel.GetComponent<MenuSkillButton>().initPanel(this, currentChar.diverSkills[i]);
            }
        }
        currentCharPointer.transform.position = memberIcons[currentCharIndex].transform.position;
    }
    public void showSkillInfo(Skill skill)
    {
        skillInfoPanel.DOMoveX(1922, 0.15f);
        skillInfo.text = skill.skillName + "\nPower: " + skill.power + "\tAccuracy: " + skill.accuracy + "\tMove: (" + skill.moveZ + "," + skill.moveY + "\nRange: " + skill.range + "\tO2: " + skill.o2 + "\n" + skill.description;
        //skill name
        //power: xxx  accuracy: xxx   move: (x, y)
        //range: x    O2: xx
        //a sentence on detail effect of this skill......


    }
    
    public void closeSkillInfo()
    {
        skillInfoPanel.DOMoveX(2600, 0.15f);
    }

    public void queryWeaponMenu()
    {
        inMenu = false;
        menuManager.weaponMenu.SetActive(true);
        gameObject.SetActive(false);
        //send current char weapon index to weaponMenuManager
        weaponMenuManager.queryOpenMenu(playerCharacterManager.characters[playerCharacterManager.teamMembers[currentCharIndex]].weaponIndex);
    }

    public void returnWeaponQuery(int result)
    {
        gameObject.SetActive(true);
        inMenu = true;
        //set result to current char's weapon index
        playerCharacterManager.characters[playerCharacterManager.teamMembers[currentCharIndex]].weaponIndex = result;
        loadCharPanel();
    }
}
