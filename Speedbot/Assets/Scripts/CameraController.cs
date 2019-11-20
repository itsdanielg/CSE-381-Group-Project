using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player, lookAt;
    public float zoomOut;
    public float mouseSensitivity;

    private Vector3 eulerRotation;

    private float lookAtOffset = 3;
    private float minViewAngle = -80;
    private float maxViewAngle = 60;
    
    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate() {

        lookAt.position = player.position + new Vector3(0, lookAtOffset, 0);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        eulerRotation.x += mouseX;
        eulerRotation.y -= mouseY;
        eulerRotation.y = Mathf.Clamp(eulerRotation.y, minViewAngle, maxViewAngle);

        Quaternion rotation = Quaternion.Euler(eulerRotation.y, eulerRotation.x, 0);
        lookAt.rotation = Quaternion.Lerp(lookAt.rotation, rotation, 1);
        transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transform.localPosition.z, -zoomOut, Time.deltaTime));
        
    }
}
