using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    public GameObject doorObject;
    

    private bool locked = true;
    private bool doorOpened = false;

    Animator animator;

    
    void Start()
    {
        animator = doorObject.GetComponent<Animator>();    
    }

    
    void Update()
    {
        if (PlayerController.Instance)
        {
            if (GameManager.HasKey && Vector3.Distance(PlayerController.Instance.transform.position, transform.position) < 2f)
            {
                locked = false;
            }
        }
        

        if (locked == false && doorOpened == false)
        {
            animator.SetBool("openDoor", true);
            doorOpened = true;
            GameManager.HasKey = false;
        }
    }
}
