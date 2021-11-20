using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public Rigidbody rb;
// -----------------------------------------------------------------------------------------------
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed;
    private  float verticalSpeed = 0f;
    private float sprintMultiplier;
    private Vector3 dash = new Vector3(100, 100, 100);
    public float gravity = 9.87f;
    public float _dashSpeed;
    public float _dashTime;
// -----------------------------------------------------------------------------------------------
    public Transform cameraHolder;
    public float mouseSensitivity;
    public float upLimit = 0;
    public float downLimit = 25;
// -----------------------------------------------------------------------------------------------

    private void Awake () {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        move();
        rotate();
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(DashCoroutine());
        }
        
    }

    private void move() {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.LeftShift))transform.Translate(new Vector3(horizontal, 0, vertical) * (sprintSpeed * Time.deltaTime));
        else transform.Translate(new Vector3(horizontal, 0, vertical) * (moveSpeed * Time.deltaTime));
        

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
        while(Time.time < startTime + _dashTime)
        {
            transform.Translate(transform.localPosition * _dashSpeed * Time.deltaTime);
            yield return null; 
        }
    }
}
