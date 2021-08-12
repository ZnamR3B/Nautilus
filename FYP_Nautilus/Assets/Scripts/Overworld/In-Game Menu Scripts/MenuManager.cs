using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public PlayerCharacterManager playerCharacterManager;
    public ItemManager itemManager;
    public WeaponManager weaponManager;
    public PlayerInfo playerInfo;

    //referencing gameobjects
    public GameObject menu;

    public GameObject teamMenu;
    public GameObject itemMenu;
    public GameObject weaponMenu;
    public GameObject questMenu;
    public GameObject mapMenu;
    public GameObject systemMenu;

    public TeamMenuManager teamMenuManager;
    public ItemMenuManager itemMenuManager;
    public WeaponMenuManager weaponMenuManager;
    public QuestMenuManager questMenuManager;
    public MapMenuManager mapMenuManager;
    public SystemMenuManager systemMenuManager;

    public GameObject playerObject;
    public PlayerController playerController;
    public PlayerInteractionController playerInteractionController;
    

    public bool inMenu;
    public bool inSubMenu;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI moneyText;
    
    private void Start()
    {
        playerCharacterManager = GetComponent<PlayerCharacterManager>();
        itemManager = GetComponent<ItemManager>();
        weaponManager = GetComponent<WeaponManager>();
        getPlayerObject();
    }
    private void Update()
    {
        if(!inMenu && !playerInteractionController.inInteraction && !inSubMenu)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                //open menu
                openMenu();
            }
        }
        else if(inMenu)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                //close menu
                closeMenu();
            }
        }
    }
    public void openMenu()
    {       
        inMenu = true;
        inSubMenu = false;
        menu.SetActive(true);
        moneyText.text = playerInfo.money.ToString() + "G";
        timeText.text = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm");
        //currentMenuIndex = 0;
        //lock player movement
        if (playerObject == null)
            getPlayerObject();
        playerInteractionController.setPlayerLock(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void openTeamMenu()
    {
        inMenu = false; 
        inSubMenu= true;
        menu.SetActive(false);
        teamMenu.SetActive(true);
        teamMenuManager.openMenu();
    }
    
    public void openItemMenu()
    {
        inMenu = false;
        inSubMenu= true;
        menu.SetActive(false);
        itemMenu.SetActive(true);
        itemMenuManager.openMenu();
    }

    public void openWeaponMenu()
    {
        inMenu = false; 
        inSubMenu= true;
        menu.SetActive(false);
        weaponMenu.SetActive(true);
        weaponMenuManager.openMenu();
    }

    public void openQuestMenu()
    {
        inMenu = false; 
        inSubMenu= true;
        menu.SetActive(false);
        questMenu.SetActive(true);
        questMenuManager.openMenu();
    }

    public void openMapMenu()
    {
        inMenu = false; 
        inSubMenu= true;
        menu.SetActive(false);
        mapMenu.SetActive(true);
        mapMenuManager.openMenu();
    }

    public void openSystemMenu()
    {
        inMenu = false; 
        inSubMenu= true;
        menu.SetActive(false);
        systemMenu.SetActive(true);
        systemMenuManager.openMenu();
    }

    public void closeMenu()
    {
        inMenu = false; 
        inSubMenu= false;
        menu.SetActive(false);
        if (playerObject == null)
            getPlayerObject();
        playerInteractionController.setPlayerLock(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void getPlayerObject()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerController = playerObject.GetComponent<PlayerController>();
        playerInteractionController = playerObject.GetComponent<PlayerInteractionController>();
    }

}
