using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody playerRB;
    private GameObject playerObject;
    private float movementSpeed;
    private float jumpCooldown;
    private float jumpHeight;
    private float timer;
    private bool isGrounded;
    private bool canJump;
    private bool hasDoubleJumped;

    private void Awake()
    {
        playerObject = GameObject.Find("Player");
        playerRB = playerObject.GetComponent<Rigidbody>();
        movementSpeed = 5f;
        jumpHeight = 500f;
        canJump = true;
        jumpCooldown = 1.2f;
    }

    private void Update()
    {
        //Check if jumpCooldown has expired and is able to jump again
        if (!canJump)
        {
            timer += Time.deltaTime;
        }
        if (timer >= jumpCooldown)
        {
            timer = 0;
            canJump = true;
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(hasDoubleJumped);

        if (Input.GetKey(KeyCode.W))
        {
            playerObject.transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerObject.transform.Translate(Vector3.left * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerObject.transform.Translate(Vector3.back * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerObject.transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            if (!isGrounded && !hasDoubleJumped)
            {
                playerRB.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode.Impulse);
                hasDoubleJumped = true;
                canJump = false;
            }
            else if (isGrounded && !hasDoubleJumped)
            {
                playerRB.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode.Impulse);
                isGrounded = false;
                canJump = false;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "JumpableSurface")
        {
            hasDoubleJumped = false;
            isGrounded = true;
        }
    }
}
