using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
// -----------------------------------------------------------------------------------------------
    [Tooltip("Move speed in meters/second")]
    public float moveSpeed;
    public float sprintSpeed;
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
// -----------------------------------------------------------------------------------------------
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Exit")
        {
            SceneManager.LoadScene(SceneManager.sceneCount + 1);
        }
    }
}
