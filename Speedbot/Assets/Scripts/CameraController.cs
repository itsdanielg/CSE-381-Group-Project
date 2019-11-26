using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private const float LOOK_AT_Y_OFFSET = 3.0f;
    private const float minViewAngle = -70.0f;
    private const float maxViewAngle = 60.0f;
    private const float zoomOut = 10;

    public Transform player, lookAt;
    public float mouseSensitivity;

    private Vector3 eulerRotation;
    
    ////////////////////////////////////////////////// START //////////////////////////////////////////////////
    // DISABLES CURSOR
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    ////////////////////////////////////////////////// LATE UPDATE //////////////////////////////////////////////////
    void LateUpdate() {

        // SET LOOK AT POSITION OF CAMERA (ABOVE PLAYER HEAD)
        lookAt.position = player.position + new Vector3(0, LOOK_AT_Y_OFFSET, 0);

        // GET DIRECTION VECTORS FROM MOUSE MOVEMENT
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // EDIT EULER ROTATION BASED ON MOUSE MOVEMENT AND CLAMP Y AXIS
        eulerRotation.x += mouseX;
        eulerRotation.y -= mouseY;
        eulerRotation.y = Mathf.Clamp(eulerRotation.y, minViewAngle, maxViewAngle);

        // ROTATE CAMERA
        Quaternion rotation = Quaternion.Euler(eulerRotation.y, eulerRotation.x, 0);
        lookAt.rotation = Quaternion.Lerp(lookAt.rotation, rotation, 1);
        transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transform.localPosition.z, -zoomOut, Time.deltaTime));
        
    }
}
