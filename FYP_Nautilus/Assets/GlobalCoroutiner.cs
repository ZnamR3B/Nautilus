using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCoroutiner : MonoBehaviour
{
    public static GlobalCoroutiner instance;

    void Start()
    {
        GlobalCoroutiner.instance = this;
    }
}