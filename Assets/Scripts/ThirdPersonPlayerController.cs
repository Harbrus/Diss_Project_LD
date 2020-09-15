using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThirdPersonPlayerController : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask ground;
    

    private Rigidbody playerRb;
    private CharacterController _controller;
    private Transform _groundChecker;
    private Vector3 _velocity;
    private bool isGrounded = true;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _groundChecker = transform.GetChild(0);
    }


    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        // check if grounded
        isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        if (isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = transform.TransformDirection(move);
        _controller.Move(move * Time.deltaTime * speed);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //// add gravity
        _velocity.y += gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            playerRb.velocity = Vector3.zero;
        }
    }
}
