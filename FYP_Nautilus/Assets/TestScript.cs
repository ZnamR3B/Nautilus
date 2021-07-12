using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    public List<int> list;
    private void Start()
    {
       for(int i = 0; i< 5; i++)
       {
            list.Add(i);
       }
       foreach(int i in list)
       {
            if(i == 2)
            {
                list.Remove(i);
            }
       }

        
    }

}
