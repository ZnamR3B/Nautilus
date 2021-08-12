using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : Interactables
{
    public GameObject shopInterface;
    public GameObject categoryLabelPrefab;
    public GameObject goodsInfoLabel;

    public int ttlPrice;
    public TextMeshProUGUI ttlPriceText;
    public Button confirmButton;

    public string[] categories;

    public Item[] items;
    public Weapon[] weapons;
    public int[] quantity;

    Transform goodsInfoHolder;
    public Transform categoriesHolder;

    public int categoryPageIndex;

    //referencing to player's info
    public ItemManager itemManager;
    public WeaponManager weaponManager;
    public PlayerInfo playerInfo;
    //player controller
    PlayerController playerController;
    PlayerInteractionController playerInteractionController;

    public override void triggerInteraction(PlayerInteractionController controller)
    {
        base.triggerInteraction(controller);
        //make cursor available
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerInteractionController = controller;
        playerController = controller.playerController;
        categoryPageIndex = 0;
        openMainMenu();
    }
    public void openMainMenu()
    {
        //set confirm buy button
        confirmButton.onClick.AddListener(delegate { confirm(); });
        GameObject infoObj = GameObject.FindGameObjectWithTag("PlayerInfo");        
        weaponManager = infoObj.GetComponent<WeaponManager>();
        itemManager = infoObj.GetComponent<ItemManager>();
        playerInfo = infoObj.GetComponent<PlayerInfo>();
        shopInterface.SetActive(true);
        //set categories
        categoriesHolder = shopInterface.transform.GetChild(0).GetChild(0);
        foreach(Transform t in categoriesHolder)
        {
            Destroy(t.gameObject);
        }
        for(int i = 0; i < categories.Length; i++)
        {
            GameObject label =
                Instantiate(categoryLabelPrefab, categoriesHolder);
            label.GetComponentInChildren<TextMeshProUGUI>().text = categories[i];
        }
        //set goods 
        goodsInfoHolder = shopInterface.transform.GetChild(0).GetChild(1).GetChild(0);
        //set money 
        shopInterface.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "remaining: " + playerInfo.money;
        openCategory();
    }

    private void Update()
    {
        if(interacting)
        {
            //in interaction
            if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                categoriesHolder.GetChild(categoryPageIndex).GetComponent<Image>().color = Color.grey;
                categoryPageIndex++;
                if (categoryPageIndex == categories.Length)
                {
                    categoryPageIndex = 0;
                }
                openCategory();
            }
            else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                categoriesHolder.GetChild(categoryPageIndex).GetComponent<Image>().color = Color.grey;
                categoryPageIndex--;
                if (categoryPageIndex < 0)
                {
                    categoryPageIndex = categories.Length - 1;
                }                
                openCategory();
            }
        }
    }
    public void openCategory()
    {
        categoriesHolder.GetChild(categoryPageIndex).GetComponent<Image>().color = Color.white;
        foreach(Transform info in goodsInfoHolder)
        {
            Destroy(info.gameObject);
        }
        if(categories[categoryPageIndex] == "items")
        {
            quantity = new int[items.Length];
            //print items in item category
            for(int i = 0; i < items.Length; i++)
            {
                GameObject info =
                    Instantiate(goodsInfoLabel, goodsInfoHolder);
                info.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = items[i].itemName;
                info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].buyPrice.ToString();
                info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = 0.ToString();
                info.GetComponent<MerchandiseButton>().initButton(i, items[i].buyPrice, this);
            }
        }
        else if (categories[categoryPageIndex] == "weapons")
        {
            quantity = new int[weapons.Length];
            //print weapons in weapon category
            for (int i = 0; i < weapons.Length; i++)
            {
                GameObject info =
                    Instantiate(goodsInfoLabel, goodsInfoHolder);
                info.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weapons[i].weaponName;
                info.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = weapons[i].buyPrice.ToString();
                info.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = 0.ToString();
                info.GetComponent<MerchandiseButton>().initButton(i, weapons[i].buyPrice, this);
            }
        }
        //add more categories here
    }


    public override void closePanel()
    {
        base.closePanel();
        shopInterface.SetActive(false);
        //end dialog
        playerInteractionController.inInteraction = false;
        playerController.canMove = true;
        playerController.cameraController.locked = false;
    }

    void confirm()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        if (playerInfo.money >= ttlPrice)
        {
            Debug.Log("confirm buy");
            if (categories[categoryPageIndex] == "items")
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if(quantity[i] > 0)
                    {
                        itemManager.itemCount[items[i].id] += quantity[i];
                    }
                }
            }
            else if (categories[categoryPageIndex] == "weapons")
            {
                for (int i = 0; i < weapons.Length; i++)
                {
                    if (quantity[i] > 0)
                    {
                        for(int j = 0; j < quantity[i]; j++)
                        {
                            weaponManager.weaponInventory.Add(weapons[i]);
                        }
                    }
                }
            }
            playerInfo.money -= ttlPrice;
        }
        closePanel();
    }

    public void updateTotalPrice(int price)
    {
        ttlPrice += price;
        ttlPriceText.text = "total: " + ttlPrice + "G";
    }
}

