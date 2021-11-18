using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
// -----------------------------------------------------------------------------------------------
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed;
    private  float verticalSpeed = 0f;
    public float gravity = 9.87f;
// -----------------------------------------------------------------------------------------------
    public Transform cameraHolder;
    public float mouseSensitivity;
    public float upLimit;
    public float downLimit;
    // -----------------------------------------------------------------------------------------------
    private Vector3 moveDirection = Vector3.zero;

    Rigidbody rb;

    Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        move();
        rotate();
    }

    private void move() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0, vertical) * (moveSpeed * Time.deltaTime));

        moveDirection.y -= gravity * Time.deltaTime;

        if (rb.velocity.x == 0f && rb.velocity.y == 0f)
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);

        }
    }

    public void rotate(){
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(0,horizontalRotation * mouseSensitivity, 0);
        cameraHolder.Rotate(-verticalRotation * mouseSensitivity, 0, 0);

        Vector3 currentRotation = cameraHolder.localEulerAngles;
        if(currentRotation.x >180) currentRotation.x -= 360;
        currentRotation.x =Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraHolder.localRotation = Quaternion.Euler(currentRotation);
    }
}
