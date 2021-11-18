using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public CharacterController characterController;
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

    private void Awake () {
        characterController = GetComponent<CharacterController>();
    }

    private void Update() {
        move();
        rotate();
    }

    private void move() {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if(characterController.isGrounded)
        {
            verticalSpeed = 0;
        } else {
            verticalSpeed -= gravity * Time.deltaTime;
        }

        Vector3 gravityMove = new Vector3(0,verticalSpeed, 0);
        Vector3 move =  transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(moveSpeed * Time.deltaTime * move + gravityMove * Time.deltaTime);
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
