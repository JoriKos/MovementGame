using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    private Transform playerBody;
    private bool cursorLocked = true;
    private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float mouseX;
    private float mouseY;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = GameObject.Find("PlayerBody").transform;
    }

    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (cursorLocked)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -85f, 85f);

            this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }

    }

    public void ToggleLockstate()
    {
        if (cursorLocked == false)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
