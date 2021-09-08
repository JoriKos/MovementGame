using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody playerRB;
    private GameObject playerObject;
    private Vector3 oppositeOfWall;
    private float movementSpeed, jumpCooldown, jumpHeight, timer;
    private bool isGrounded, canJump, hasDoubleJumped, canWallRun, isWallRunning, isJumping, isMoving, isRunning; //Movement checks


    private void Awake()
    {
        playerObject = GameObject.Find("PlayerBody");
        playerRB = playerObject.GetComponent<Rigidbody>();
        movementSpeed = 5f;
        jumpHeight = 500f;
        canJump = true;
        jumpCooldown = 1.2f;
        canWallRun = false;
        isJumping = false;
    }

    private void Update()
    {
        #region JumpCheckAndCooldown
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
        #endregion
        #region ButtonCheck
        if (Input.GetKey(KeyCode.Space) && canJump)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        #endregion
        if (Input.GetKeyDown(KeyCode.Space) && canWallRun)
        {
            isWallRunning = true;
        }
        else if (!canWallRun)
        {
            isWallRunning = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
    }
    private void FixedUpdate()
    {
        //Debug.Log(isGrounded);
        #region WASDMovement
        if (isMoving)
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
                playerObject.transform.Translate(Vector3.right * (movementSpeed - 1.5f) * Time.deltaTime);
            }
        }
        #endregion
        #region Jump
        if (isJumping)
        {
            if (!isGrounded && !hasDoubleJumped) //Double jump
            {
                playerRB.AddForce(Vector3.up * (jumpHeight + 100f) * Time.deltaTime, ForceMode.Impulse);
                hasDoubleJumped = true;
                canJump = false;
            }
            else if (isGrounded && !hasDoubleJumped) //Initial jump
            {
                playerRB.AddForce(Vector3.up * jumpHeight * Time.deltaTime, ForceMode.Impulse);
                isGrounded = false;
                canJump = false;
            }
        }
        #endregion
        #region WallRunning
        if (isWallRunning)
        {
            playerRB.AddForce(Vector3.up * 10f * Time.deltaTime);
            playerRB.AddForce(Vector3.forward * (movementSpeed * 10f) * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRB.AddForce(oppositeOfWall * (movementSpeed * 2500f) * Time.deltaTime);
                playerRB.AddForce(Vector3.up * (movementSpeed * 2500f) * Time.deltaTime);
            }
        }
        #endregion
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "JumpableSurface")
        {
            hasDoubleJumped = false;
            isGrounded = true;
        }
        //Determines whether or not you can wallrun. (Is touching wall you can run on = yes. Isn't touching the ground = yes)
        //Returns that player is no longer on the ground, can no longer 
        if (collision.gameObject.tag == "RunnableWall" && !isGrounded)
        {
            if (this.transform.position.x < collision.gameObject.transform.position.x) //If wall is to the right
            {
                oppositeOfWall = Vector3.left;
            }
            else if (this.transform.position.x > collision.gameObject.transform.position.x) //If wall is to the left
            {
                oppositeOfWall = Vector3.right;
            }

            canWallRun = true;
            isGrounded = false;
            playerRB.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "RunnableWall")
        {
            canWallRun = false;
            playerRB.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        if (collision.gameObject.tag == "JumpableSurface")
        {
            isGrounded = false;
        }
    }
}