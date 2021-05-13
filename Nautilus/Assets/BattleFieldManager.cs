using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleFieldManager : MonoBehaviour
{
    //reference to other manager
    UnitManager unitManager;
    //map
    LayerMask tileMask;
    Tile[] mapTiles;
    GameObject[] tileObjects;
    int[] tilePattern;

    //deployment
    bool unitPanelShown = false;
    public Transform unitPanel;
    LayerMask deployPreviewPlaneMask;
    GameObject currentChosenUnit;

    public GameObject iconPrefab;
    public Transform unitIconHolder;

    private void Start()
    {
        tileMask = LayerMask.GetMask("Tile");
        unitManager = GameObject.FindGameObjectWithTag("UnitMangaer").GetComponent<UnitManager>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(camRay, out hit, Mathf.Infinity, tileMask))
            {
                if(hit.collider.GetComponentInParent<Tile>().deployable)
                {
                    //deploy unit
                }
            }
        }
        if(currentChosenUnit != null)
        {
            RaycastHit hit;
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay, out hit, Mathf.Infinity, deployPreviewPlaneMask))
            {
                currentChosenUnit.transform.position = hit.point;
            }
        }
    }

    public void chooseUnit(int index)
    {
        currentChosenUnit =
            Instantiate(unitManager.unitObjects[index]);
    }

    public void initUnitPanel()
    {
        int unitCount = unitManager.unitCount.Length;
        for (int i = 0; i < unitCount; i++) 
        {
            GameObject thisIcon =
                Instantiate(iconPrefab, unitIconHolder);
            thisIcon.GetComponent<Image>().sprite = unitManager.unitObjects[i].GetComponent<BattleUnit>().unitIcon;
            thisIcon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = unitManager.unitObjects[i].GetComponent<BattleUnit>().cost.ToString();
            thisIcon.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = unitManager.unitCount[i].ToString();
        }
    }

    public void triggerUnitListPanel()
    {
        if(unitPanelShown)
        {
            //close panel
            unitPanel.GetComponent<Animator>().SetBool("show",false);
        }
        else
        {
            //open panel
            unitPanel.GetComponent<Animator>().SetBool("show", true);
        }
        unitPanelShown = !unitPanelShown;
    }

}
