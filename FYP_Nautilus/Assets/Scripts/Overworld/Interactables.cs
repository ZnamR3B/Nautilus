using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public GameObject playerCam;

    public GameObject interactionHint;

    public GameObject canvasGroup;

    public bool showHint;

    public bool interacting;

    public bool lookAtPlayer;

    public void findPlayercam()
    {
        playerCam = FindObjectOfType<CameraController>().gameObject;
    }

    public void showInteractionHint()
    {
        interactionHint.SetActive(true);
    }

    public void hideInteractionHint()
    {
        interactionHint.SetActive(false);
    }

    private void LateUpdate()
    {
        if (playerCam == null)
        {
            findPlayercam();
        }
        if(interacting)
        {
            if(Input.GetButtonDown("Cancel"))
            {
                closePanel();
            }
                
        }
        if(lookAtPlayer)
        {
            canvasGroup.transform.LookAt(playerCam.transform);
        }
        if (!showHint)
            hideInteractionHint();
        else
            showInteractionHint();
        showHint = false;
    }

    public virtual void triggerInteraction(PlayerInteractionController controller)
    {
        interacting = true;
    }

    public virtual void closePanel()
    {
        interacting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
