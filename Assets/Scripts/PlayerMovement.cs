using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float speed = 9.0f;

    Rigidbody rb;
    Animator animator;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontal, 0, vertical) * (speed * Time.deltaTime));

        moveDirection.y -= gravity * Time.deltaTime;

        if(rb.velocity.x == 0f && rb.velocity.y == 0f)
        {
            animator.SetBool("isWalking", false);
        } else
        {
            animator.SetBool("isWalking", true);

        }
    }
}
