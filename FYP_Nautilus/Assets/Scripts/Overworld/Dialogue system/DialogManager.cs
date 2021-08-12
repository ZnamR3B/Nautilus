using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public PlayerInteractionController playerInteractionController;
    public PlayerController playerController;
    //UI reference
    public TextMeshProUGUI dialogText;

    public GameObject dialogPanelObject;

    public Image speakChar;

    public bool inDialog;

    private Queue<string> sentences;

    public void startDialog(Dialog dialog)
    {
        inDialog = true;
        dialogPanelObject.SetActive(true);
        speakChar.sprite = dialog.speaker;
        sentences = new Queue<string>();
        foreach (string s in dialog.sentences)
        {
            sentences.Enqueue(s);
        }        
        
    }

    public IEnumerator typeSentence(string sentence)
    {
        dialogText.text = "";
        foreach(char text in sentence)
        {
            dialogText.text += text;
            yield return new WaitForEndOfFrame();
        }
    }
    public void displayNextSentence()
    {
        //end dialog
        if(sentences.Count == 0)
        {
            endDialog();
        }
        else
        {
            //show dialog
            string sentence = sentences.Dequeue();
            //if last sentence is not finished, stop it and jump to the next sentence
            StopAllCoroutines();
            StartCoroutine(typeSentence(sentence));
        }
    }

    private void Update()
    {
        if (inDialog && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            displayNextSentence();
        }
    }

    void endDialog()
    {
        //end dialog
        inDialog = false;
        dialogPanelObject.SetActive(false);
        playerInteractionController.inInteraction = false;
        playerController.canMove = true;
        playerController.cameraController.locked = false;
    }
}
