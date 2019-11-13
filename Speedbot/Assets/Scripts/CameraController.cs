using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target, pivot, lookAt;
    public Vector3 offset;
    public bool useOffset;
    public float rotationSpeed;

    private float mouseX, mouseY;
    private float minViewAngle = -70;
    private float maxViewAngle = 20;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (useOffset) {
            offset = target.position - transform.position;
        }
        pivot.transform.position = target.transform.position;
        pivot.transform.parent = null;
    }

    void LateUpdate() {

        pivot.transform.position = target.transform.position;

        mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        //mouseY = Mathf.Clamp(mouseY, -70, 50);

        pivot.Rotate(0, mouseX, 0);
        pivot.Rotate(-mouseY, 0, 0);

        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f) {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }
        if (pivot.rotation.eulerAngles.x > 180 && pivot.rotation.eulerAngles.x < 360f + minViewAngle) {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0, 0);
        }

        float yAngle = pivot.eulerAngles.y;
        float xAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(lookAt);

        
        // Player.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
