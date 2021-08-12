using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    public int toScene;
    public int toIndex;

    public static TransferManager _instance;

    public Animator canvasAnim;

    public GameObject playerPrefab;

    AsyncOperation asyncLoadLevel;
    private void Start()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator transfer(GameObject p)
    {
        GameObject player = null;
        if(p != null)
        {
            player = p;
        }
        Debug.Log(SceneManager.GetActiveScene().buildIndex != toScene);
        if (SceneManager.GetActiveScene().buildIndex != toScene)
        {
            SceneManager.LoadScene(toScene, LoadSceneMode.Single);
            yield return StartCoroutine(afterLoadScene());                
        }
        else
        {
            canvasAnim.SetTrigger("transit");
            yield return new WaitForSeconds(1 / 3);
            Transform data = GameObject.FindGameObjectWithTag("TransferData").transform.GetChild(toIndex);
            Debug.Log("transfer");
            player.transform.position = data.position;
            player.transform.localRotation = data.localRotation;
        }

    }

    public IEnumerator afterLoadScene()
    {
        while(SceneManager.GetActiveScene().buildIndex != toScene)
        {
            yield return null;
        }
        GameObject[] oldPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject old in oldPlayers)
        {
            Destroy(old);
        }
        GameObject player = Instantiate(playerPrefab);
        canvasAnim.SetTrigger("transit");
        yield return new WaitForSeconds(1 / 3);
        Transform data = GameObject.FindGameObjectWithTag("TransferData").transform.GetChild(toIndex);
        Debug.Log(data.gameObject);
        player.transform.position = data.position;
        player.transform.localRotation = data.localRotation;
        transform.parent.GetComponent<MenuManager>().getPlayerObject();
    }
}
