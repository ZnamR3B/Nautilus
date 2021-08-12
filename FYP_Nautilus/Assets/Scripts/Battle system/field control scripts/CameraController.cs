using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    
    public bool underwater;

    public float originalCamDist;
    public Transform target;
    public Transform player;
    public float camSpeed;

    public float rotX, rotY;

    public float rotYMin, rotYMax;

    private Vector3 offset;

    public bool locked;

    LayerMask nonBlockMask;
    private void Start()
    {
        nonBlockMask = LayerMask.GetMask("Block","Floor");
        //nonBlockMask = ~nonBlockMask;
        offset = transform.position - player.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {        
        if(!locked)
        {
            //transform.position = player.position + offset;
            rotX += Input.GetAxis("Mouse X") * camSpeed;
            rotY -= Input.GetAxis("Mouse Y") * camSpeed;

            rotY = Mathf.Clamp(rotY, rotYMin, rotYMax);

            transform.LookAt((target));

            target.rotation = Quaternion.Euler(rotY, rotX, 0);

            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, Mathf.Abs(originalCamDist), nonBlockMask))
            {
                //Debug.Log("hit on wall, hit on: " + hit.collider.gameObject);
                transform.position = hit.point + (player.transform.position - hit.point).normalized * .25f;

            }
            else
            {
                //Debug.Log("no hit");
                transform.localPosition = new Vector3(0, 0, originalCamDist);
            }
        }
    }
}
