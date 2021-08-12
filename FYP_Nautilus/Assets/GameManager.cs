using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public GameObject playerPrefab;

    public GameObject tempCam;

    private void Awake()
    {
        Destroy(tempCam);
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Transform data = GameObject.FindGameObjectWithTag("TransferData").transform.GetChild(0);
                Instantiate(playerPrefab, data.transform.position, Quaternion.identity);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
