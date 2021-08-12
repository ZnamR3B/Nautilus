using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public DialogManager dialogManager;
    public PlayerController playerController;
    //canvas object reference
    public GameObject interactionHint;

    public Interactables targetInteractable;

    public CameraController cam;

    public float interactDist;

    public bool inInteraction;

    public bool inMenu;

    LayerMask interactableMask;

    private void Start()
    {
        interactableMask = LayerMask.GetMask("Interactable");        
        inInteraction = false;
    }
    void Update()
    {
        //calculate forward vector of camera
        Vector3 forward = cam.target.forward;
        forward.y = 0;
        //ray direction        
        Ray camRay = new Ray(transform.position, forward);        
        RaycastHit hit;
        if (Physics.SphereCast(camRay, .25f, out hit, interactDist, interactableMask))
        {
            interactionHint.SetActive(true);
            hit.collider.GetComponent<Interactables>().showHint = true;
            targetInteractable = hit.collider.GetComponent<Interactables>();
        }
        else
        {
            interactionHint.SetActive(false);
            targetInteractable = null;
        }
        if(targetInteractable && Input.GetMouseButtonDown(1) && !inInteraction)
        {
            //trigger interactables
            setPlayerLock(true);
            inInteraction = true;
            targetInteractable.triggerInteraction(this);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Door"))
        {
            other.GetComponent<Animator>().SetBool("character_nearby", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {  
        if (other.CompareTag("Door"))
        {
            other.GetComponent<Animator>().SetBool("character_nearby", false);
        }
    }

    public void setPlayerLock(bool toLock)
    {        
        playerController.canMove = !toLock;
        playerController.cameraController.locked = toLock;
    }
}
