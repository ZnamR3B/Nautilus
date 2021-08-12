using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenuPointButton : MonoBehaviour
{
    MenuManager menuManager;
    MapMenuManager mapManager;
    TransferManager transferManager;
    public AreaPoint toPoint;
    public void initButton(TransferManager tranMan, MenuManager menuMan, MapMenuManager mapMan, AreaPoint point)
    {
        mapManager = mapMan;
        menuManager = menuMan;
        transferManager = tranMan;
        toPoint = point;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(onClick()); });
    }

    IEnumerator onClick()
    {
        transferManager.toScene = toPoint.sceneIndex;
        transferManager.toIndex = toPoint.placeIndex;
        yield return StartCoroutine(transferManager.transfer(menuManager.playerObject));
        mapManager.closeMenu();
        menuManager.closeMenu();
    }
}
