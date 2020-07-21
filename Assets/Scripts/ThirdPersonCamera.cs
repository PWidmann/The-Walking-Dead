using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public static ThirdPersonCamera Instance;

    public bool lockCursor;
    public float mouseSensitivity = 5;
    public Transform target;
    public float maxDistanceFromTarget = 4;
    public float minDistanceFromTarget = 0.5f;

    public float currentDistanceFromTarget;
    public Vector2 pitchMinMax = new Vector2(0, 85);

    [SerializeField] Camera cam;

    public float rotationSmoothTime = 0.08f;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public LayerMask collisionLayer;

    // Left / Right
    float yaw = 45f;
    // Up / Down
    float pitch = 40f;
    
    


    private void Start()
    {
        if (Instance == null)
            Instance = this;
        currentDistanceFromTarget = maxDistanceFromTarget;
    }

    private void Update()
    {
        
    }

    void LateUpdate()
    {
        if (true)
        {
            //yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            //pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * -1;
            //pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);


            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;
            
            if(target)
                transform.position = target.position - transform.forward * currentDistanceFromTarget;
        }
    }

    


}
