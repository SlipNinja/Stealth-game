using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
   
{
    public AK.Wwise.Event MusicStop;
    public AK.Wwise.Event MusicStart;
    public AK.Wwise.Event HeartbeatStop;
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

    public GameObject deathMenu;
    public GameObject winScreen;

    private bool caught;// Used for Wwise sound

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        caught = false;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if(other.gameObject.tag == "Dog")
        {
            Time.timeScale = 0f;
            deathMenu.SetActive(true);
            caught = true;
            MusicStart.Post(gameObject);
            MusicStop.Post(gameObject);
            HeartbeatStop.Post(gameObject);
        }
        if(other.gameObject.tag == "Completed")
        {
            Time.timeScale = 0f;
            winScreen.SetActive(true);
        }
       
    }
}
