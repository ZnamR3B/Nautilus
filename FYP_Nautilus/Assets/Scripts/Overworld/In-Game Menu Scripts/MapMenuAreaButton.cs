using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapMenuAreaButton : MonoBehaviour
{
    public MapMenuManager manager;
    public int areaIndex;

    public void initButton(MapMenuManager m, int a)
    {
        manager = m;
        areaIndex = a;
        GetComponent<Button>().onClick.AddListener(delegate { onClick(); });
    }

    void onClick()
    {
        manager.openArea(areaIndex);
    }
}
