using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransferType { sceneChange, positionChange};

public class TransferBox : MonoBehaviour
{
    public int sceneIndex;
    public int destinationIndex;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TransferManager._instance.toIndex = destinationIndex;
            TransferManager._instance.toScene = sceneIndex;
            StartCoroutine(TransferManager._instance.transfer(other.gameObject));
        }
    }
}
