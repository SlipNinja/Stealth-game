using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
// -----------------------------------------------------------------------------------------------
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed;
    public float sprintSpeed;
    public float gravity = 9.87f;
    public float dashTime;
    public float dashSpeed;
// -----------------------------------------------------------------------------------------------
    public Transform cameraHolder;
    public float mouseSensitivity;
    public float upLimit = 0;
    public float downLimit = 25;
// -----------------------------------------------------------------------------------------------
    private Vector3 moveDirection = Vector3.zero;
    Rigidbody rb;
    // Animator animator;

    private void Awake () {
        rb = GetComponent<Rigidbody>();
        // animator = GetComponent<Animator>();
    }

    private void Update() {
        move();
        rotate();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(DashCoroutine());
            Debug.Log("dash");
        }
        
    }

    private void move() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        switch(Input.GetKey(KeyCode.LeftShift))
        {
            case true:
                transform.Translate(new Vector3(horizontal, 0, vertical) * (sprintSpeed * Time.deltaTime));
                break;
            case false:
                transform.Translate(new Vector3(horizontal, 0, vertical) * (moveSpeed * Time.deltaTime));
                break;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        // if (rb.velocity.x == 0f && rb.velocity.y == 0f)
        // {
        //     animator.SetBool("isWalking", false);
        // }
        // else
        // {
        //     animator.SetBool("isWalking", true);
        // }
    }

    private void rotate(){
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(0,horizontalRotation * mouseSensitivity, 0);
        cameraHolder.Rotate(-verticalRotation * mouseSensitivity, 0, 0);

        Vector3 currentRotation = cameraHolder.localEulerAngles;
        if(currentRotation.x >180) currentRotation.x -= 360;
        currentRotation.x =Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraHolder.localRotation = Quaternion.Euler(currentRotation);
    }

    private IEnumerator DashCoroutine()
    {
        float startTime = Time.time;
        while(Time.time < startTime + dashTime)
        {
            transform.Translate(transform.localPosition * dashSpeed * Time.deltaTime);
            yield return null; 
        }
    }
}
