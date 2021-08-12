using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautilusMenuManager : MonoBehaviour
{
    GameObject playerObject;

    public GameObject mainMenu;
    public GameObject oceanMap;
    public GameObject teamMemberMap;

    public PlayerInfo playerInfo;
    public NautilusOceanManager nautilusOceanManager;

    public Transform exit;

    int mapIndex; // 0 = ronda
    bool inMenu;

    
    public void openOceanMap()
    {
        nautilusOceanManager.openMenu(mapIndex, playerInfo);
        mainMenu.SetActive(false);
        teamMemberMap.SetActive(false);
    }

    public void openMemberChangeMenu()
    {

    }

    public void openMainMenu()
    {
        if (playerInfo == null)
            playerInfo = FindObjectOfType<PlayerInfo>();
        mapIndex = playerInfo.mapIndex;
        mainMenu.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObject = other.gameObject;
            other.GetComponent<PlayerInteractionController>().setPlayerLock(true);
            openMainMenu();
        }
    }

    public void closeMenu()
    {
        mainMenu.SetActive(false);
        playerObject.transform.position = exit.position;
        playerObject.transform.localRotation = exit.localRotation;
    }
}
