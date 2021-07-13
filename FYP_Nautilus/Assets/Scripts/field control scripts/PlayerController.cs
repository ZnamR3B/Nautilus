using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool underwater = false;
    public bool inBattle = false;
    public float speed;
    float walkSpeed = 8;
    float swimSpeed = 5;
    public float jumpForce;

    public CameraController cameraController;

    private float h;
    private float v;
    private float up;

    Animator anim;
    Rigidbody rb;

    public bool onGround = true;
    bool jumpPressing = false;

    public float jumpTimer = 0;

    LayerMask floorMask;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
        rb.drag = 0.1f;
        inBattle = false;
    }

    private void Update()
    {       
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        up = Input.GetAxis("Jump");
    }

    private void FixedUpdate()
    {
        if(!inBattle)
        {
            //check player touched the ground or not
            if (!onGround)
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position - new Vector3(0, 1, 0), 0.01f, floorMask, QueryTriggerInteraction.Ignore);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if (hitColliders[i].CompareTag("Floor"))
                    {
                        onGround = true;
                        Debug.Log("Floored");
                        speed = walkSpeed;
                        break;
                    }
                }
            }

            //set animator bool : walking 
            if (h != 0 || v != 0)
            {
                anim.SetBool("Moving", true);
            }
            else
            {
                anim.SetBool("Moving", false);
            }

            //movement
            if (underwater)
            {
                //underwater movement: 

                //get camera rotation (forward & right)    
                Vector3 forward = cameraController.target.forward;
                Vector3 right = cameraController.target.right;
                forward.Normalize();
                right.Normalize();
                //move base on this direction
                Vector3 moveDirection = forward * v + right * h;
                //get movement direction
                Vector3 movement = moveDirection * (Time.deltaTime * speed);
                //set rotation
                if (h != 0 || v != 0)
                {
                    //if moving
                    transform.rotation = Quaternion.LookRotation(moveDirection);

                }
                else
                {
                    //if not moving, reset all rotations except y-axis (facing direction)
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
                //move player object
                transform.Translate(movement, Space.World);

                //move player upward
                Vector3 upwardMovement = new Vector3(0, up, 0) * (Time.deltaTime * speed);
                transform.Translate(upwardMovement, Space.World);
            }
            else
            {
                Vector3 forward = cameraController.target.forward;
                forward.y = 0;
                Vector3 right = cameraController.target.right;
                right.y = 0;
                forward.Normalize();
                right.Normalize();
                Vector3 moveDirection = forward * v + right * h;
                moveDirection.y = 0;
                Vector3 movement = moveDirection * (Time.deltaTime * speed);

                if (h != 0 || v != 0)
                {
                    transform.rotation = Quaternion.LookRotation(moveDirection);
                }
                transform.Translate(movement, Space.World);

                if (onGround && Input.GetButton("Jump") && !jumpPressing)
                {
                    Debug.Log("jump");
                    onGround = false;
                    rb.velocity += Vector3.up * jumpForce;
                    speed = walkSpeed / 2;
                }
                if (rb.velocity.y > 0)
                {
                    rb.velocity -= new Vector3(0, jumpForce, 0) * Time.deltaTime;
                }
                if (rb.velocity.y < 0)
                {
                    rb.velocity -= new Vector3(0, jumpForce * 2, 0) * Time.deltaTime;
                }
                if (Input.GetButton("Jump"))
                {
                    jumpPressing = true;
                }
                else
                {
                    jumpPressing = false;
                }
            }

        }

    }

    

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Water"))
        {
            underwater = true;
            anim.SetBool("Underwater", true);
            rb.drag = 10;
            speed = swimSpeed;
            onGround = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //underwater = false;            
            rb.drag = 0.1f;            
        }
        if(other.CompareTag("Floor"))
        {
            anim.SetBool("Underwater", false);
            underwater = false;
            //speed = walkSpeed;
        }
    }


}
