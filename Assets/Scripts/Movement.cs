using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRB;
    private GameObject playerObject;
    private Vector3 oppositeOfWall;
    private float movementSpeed, jumpCooldown, jumpHeight, timer, sprintSpeed, wallrunTimer;
    private bool isGrounded, canJump, hasDoubleJumped, canWallRun, isWallRunning, isJumping, isMoving, isRunning; //Movement checks


    private void Awake()
    {
        playerObject = GameObject.Find("PlayerBody");
        playerRB = playerObject.GetComponent<Rigidbody>();
        jumpHeight = 500f;
        movementSpeed = 0;
        canJump = true;
        jumpCooldown = 1.2f;
        canWallRun = false;
        isJumping = false;
        sprintSpeed = 0;
        playerRB.useGravity = true;
    }

    private void Update()
    {
        Debug.Log(playerRB.velocity);

        #region MovementMinMax
        #region X
        if (playerRB.velocity.x >= 10)
        {
            playerRB.velocity = new Vector3(10, playerRB.velocity.y, playerRB.velocity.z);
        }

        if(playerRB.velocity.x <= -10)
        {
            playerRB.velocity = new Vector3(-10, playerRB.velocity.y, playerRB.velocity.z);
        }
        #endregion

        #region Y
        if (playerRB.velocity.y > 10)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, 10, playerRB.velocity.z);
        }

        if (playerRB.velocity.y < -10)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, -10, playerRB.velocity.z);
        }
        #endregion

        #region Z
        if (playerRB.velocity.z > 15 && !isRunning)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y, 15);
        }

        if (playerRB.velocity.z > 20 && isRunning)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, playerRB.velocity.y, 20);
        }
    
        if(playerRB.velocity.z < -20)
        {
            playerRB.velocity = new Vector3(0, 0, -20);
        }
        #endregion
        #endregion

        #region MovementModifier
        //When actively holding W/A/S/D, increase movement speed (Maximum of 5)
        if (isMoving)
        {
            movementSpeed += 3.5f;
            if (movementSpeed >= 5f)
            {
                movementSpeed = 5f;
            }
        }

        //When not actively holding W/A/S/D, reduce movement speed (Minimum of 0)
        if (movementSpeed > 0f && !isMoving)
        {
            movementSpeed -= 3.5f;
            if (movementSpeed <= 0f)
            {
                movementSpeed = 0f;
            }
        }
        #endregion

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

        #region WallRunning
        if (Input.GetKeyDown(KeyCode.Space) && canWallRun)
        {
            isWallRunning = true;
        }
        else if (!canWallRun)
        {
            isWallRunning = false;
        }
        #endregion

        #region Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isWallRunning)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region Bullshit
        //I hate this
        //if()
        #endregion

        #region WASDMovement
        if (isMoving)
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerRB.AddForce(this.transform.forward * ((movementSpeed * 150) + sprintSpeed) * Time.deltaTime);
                if (isJumping && !isWallRunning)
                {
                    playerRB.AddForce(-this.transform.up * 10);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                playerRB.AddForce(-this.transform.right * (movementSpeed * 150) * Time.deltaTime);
                if (isJumping && !isWallRunning)
                {
                    playerRB.AddForce(-this.transform.up * 10);
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                playerRB.AddForce(-this.transform.forward * (movementSpeed * 150) * Time.deltaTime);
                if (isJumping && !isWallRunning)
                {
                    playerRB.AddForce(-this.transform.up * 10);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                playerRB.AddForce(this.transform.right * (movementSpeed * 150) * Time.deltaTime);
                if (isJumping && !isWallRunning)
                {
                    playerRB.AddForce(-this.transform.up * 10);
                }
            }
        }
        #endregion

        #region Jump
        if (isJumping)
        {
            if (!isGrounded && !hasDoubleJumped) //Double jump
            {
                playerRB.AddForce(Vector3.up * (jumpHeight + 100f) * Time.deltaTime, ForceMode.Impulse); //Upwards force
                //playerRB.AddForce(this.transform.forward * 10f * Time.deltaTime, ForceMode.Impulse); //Forward force
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
            //playerRB.AddForce(Vector3.up * Physics.gravity.x * Time.deltaTime); //Upwards force to prevent falling down
            //playerRB.AddForce(Vector3.forward * (movementSpeed * 10f) * Time.deltaTime);
            if (Input.GetKey(KeyCode.Space))
            {
                playerRB.AddForce(oppositeOfWall * (movementSpeed * 2500f) * Time.deltaTime);
                playerRB.AddForce(Vector3.up * (movementSpeed * 2500f) * Time.deltaTime);
            }
        }
        #endregion

        #region Sprinting
        if (isRunning)
        {
            sprintSpeed = 5;
        }
    
        if (!isRunning)
        {
            sprintSpeed = 0;
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