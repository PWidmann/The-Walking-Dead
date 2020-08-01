﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    //Movement
    [Header("Movement")]
    public Vector2 inputDir;
    public Vector3 velocity;
    
    float walkSpeed = 3;
    float runSpeed = 6;
    float speed;
    float currentSpeed;
    public float gravity = -12;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    
    float velocityY;

    public Vector3 position;

    public bool canMove = false;


    Camera cam;
    Transform cameraT;
    Transform cameraTarget;
    bool cameraTargetSet = false;


    public CharacterController controller;
    Animator animator;

    void Start()
    {
        if (Instance == null || Instance != this)
            Instance = this;
        // References
        cam = Camera.main;
        cameraT = cam.transform;
        animator = GetComponentInChildren<Animator>();

        inputDir = Vector2.zero;
        canMove = true;
        speed = walkSpeed;
    }

    void Update()
    {
        position = transform.position;

        //Movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetButton("JoystickButtonA"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        Move(inputDir);

        // animator
        float animationSpeedPercent = currentSpeed / runSpeed;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void Move(Vector2 inputDir)
    {
        //Gravity
        velocityY += Time.deltaTime * gravity;

        //Player rotation
        if (canMove && inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        // Smooth runspeed 
        float targetSpeed = speed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        
        velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        if (canMove)
        {
            controller.Move(velocity * Time.deltaTime);
        }
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;


        // Grounded
        if (controller.isGrounded)
        {
            velocityY = 0;
            animator.SetBool("nearGround", true);
            

            if (cameraTargetSet == false)
            {
                cam.transform.GetComponent<ThirdPersonCamera>().target = GameObject.Find("CameraTarget").transform;
                cameraTargetSet = true;
            }
            
        }
    }
}
