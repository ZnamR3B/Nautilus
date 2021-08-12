using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class ItemMenuManager : MonoBehaviour
{
    public MenuManager menuManager;
    public ItemManager itemManager;
    public PlayerCharacterManager playerCharacterManager;

    public GameObject itemIconPrefab;
    public Transform itemIconHolder;
    public GameObject charPanelPrefab;
    public Transform charPanelsHolder;
    public GameObject infoPanel;
    bool inMenu;
    [SerializeField]
    int maxIconInPage;
    int pageIndex;

    bool showingItemInfo;
   
    
    void Update()
    {
        if (inMenu)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.D))
            {
                pageIndex++;
                loadPage();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                pageIndex--;
                loadPage();
            }
            if (Input.GetButtonDown("Cancel"))
            {
                if(showingItemInfo)
                {
                    closeItemInfo();
                }
                else
                {
                    //close menu and return to main menu
                    closeMenu();
                }
            }
        }
    }
    public void openMenu()
    {
        //init item menu
        inMenu = true;
        pageIndex = 0;
        loadPage();
        //load items
        foreach (Transform item in itemIconHolder)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < itemManager.items.Length; i++)
        {
            if(itemManager.itemCount[i] > 0)
            {
                //load that item
                GameObject itemIcon =
                    Instantiate(itemIconPrefab, itemIconHolder);
                itemIcon.GetComponent<ItemMenuIcon>().initIcon(this, itemManager.items[i]);
                itemIcon.GetComponent<Image>().sprite = itemManager.items[i].icon;
                itemIcon.GetComponentInChildren<TextMeshProUGUI>().text = itemManager.itemCount[i].ToString();
            }
        }

        //init characters list
        charPanelsHolder.transform.DOMoveX(-827, 0.15f);
        //clear list before
        foreach (Transform obj in charPanelsHolder)
        {
            Destroy(obj.gameObject);
        }
        //load current team character
        for(int i = 0; i < playerCharacterManager.teamMembers.Length; i++)
        {
            Character currentChar = playerCharacterManager.characters[playerCharacterManager.teamMembers[i]];
            GameObject panel =
                Instantiate(charPanelPrefab, charPanelsHolder);
            panel.GetComponent<Image>().color = (currentChar.remainHP > 0) ? Color.white : Color.black;
            //panel.GetComponent<SwitchCharButton>().initButton(ally, this, ally.alive && ally != battleSystem.allyChar[battleSystem.currentCharIndex]);
            panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = currentChar.characterName;
            panel.transform.GetChild(1).GetComponent<Slider>().value = (float)currentChar.remainHP / (currentChar.base_HP + (currentChar.equippedWeapon != null ? currentChar.equippedWeapon.extraHP : 0));
            panel.transform.GetChild(2).GetComponent<Slider>().value = (float)currentChar.remainO2 / currentChar.base_O2 + (currentChar.equippedWeapon != null ? currentChar.equippedWeapon.extraO2: 0);
            panel.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = currentChar.remainHP + " / " + (currentChar.base_HP + (currentChar.equippedWeapon != null ? currentChar.equippedWeapon.extraHP : 0)).ToString() ;
            panel.transform.GetChild(4).GetComponent<Image>().sprite = currentChar.icon;
        }
    }

    void loadPage()
    {
        //close info panel
        closeItemInfo();
        //move the panel to show the corresponding page
        itemIconHolder.transform.DOMoveX(-1920 * pageIndex, 0.15f);
    }

    public void showItemInfo(Item item)
    {
        //open (and move) the info panel
        showingItemInfo = true;
        infoPanel.GetComponent<RectTransform>().DOMoveX(1922, 0.05f);
        infoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
        infoPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.description;
        infoPanel.transform.GetChild(2).gameObject.SetActive(item.usable_field);
        infoPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { useItem(item); });
    }

    void useItem(Item item)
    {
        if(item.usable_field )
        {
            if(item.toTarget)
            {
                //choose target and use item to it
            }
            else
            {
                //do effect
            }
        }
    }

    public void closeItemInfo()
    {
        //close info panel
        infoPanel.GetComponent<RectTransform>().DOMoveX(2622, 0.05f);
        showingItemInfo = false;
    }
    void closeMenu()
    {
        inMenu = false;
        gameObject.SetActive(false);
        menuManager.openMenu();
    }
}
