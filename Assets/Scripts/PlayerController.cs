using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    //Movement
    [Header("Movement")]
    public Vector2 inputDir;
    public Vector3 velocity;
    
    public float speed = 0;
    public float walkSpeed = 2;
    public float runSpeed = 4;
    public float gravity = -12;
    public float jumpHeight = 1;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    public float currentSpeed;
    float velocityY;

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
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        speed = walkSpeed;

        inputDir = Vector2.zero;

        
    }

    void Update()
    {
        if (canMove)
        {
            //Movement
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            inputDir = input.normalized;
            
        }

        Move(inputDir);

        // animator
        float animationSpeedPercent = currentSpeed / runSpeed;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("JoystickButtonA"))
        {
            speed = runSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetButtonUp("JoystickButtonA"))
        {
            speed = walkSpeed;
        }

    }

    void Move(Vector2 inputDir)
    {
        //Gravity
        velocityY += Time.deltaTime * gravity;

        //Player rotation
        if (inputDir != Vector2.zero && canMove)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        // Smooth runspeed 
        float targetSpeed = speed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);


        velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        
        controller.Move(velocity * Time.deltaTime);
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
