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

    private void Awake()
    {
        playerObject = GameObject.Find("Player");
        playerRB = playerObject.GetComponent<Rigidbody>();
        movementSpeed = 5f;
        jumpHeight = 50f;
        isGrounded = true;
        jumpCooldown = 1.5f;
    }

    private void Update()
    {
        if (!isGrounded)
        {
            timer += Time.deltaTime;
        }
        if (timer >= jumpCooldown)
        {
            timer = 0;
            isGrounded = true;
        }

        Debug.Log(isGrounded);   
    }

    private void FixedUpdate()
    {
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

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            playerRB.AddForce(Vector3.up * jumpHeight * Time.deltaTime);
            isGrounded = false;
        }
    }
}
