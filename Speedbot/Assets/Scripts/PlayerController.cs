using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController controller;
    public Animator animator;
    public Transform pivot;
    public GameObject playerModel;

    public float walkSpeed;
    public float runningSpeed;
    public float jumpRunMultiplier;
    public float jumpHeight;
    public float gravity;
    public float rotationSpeed;

    private Vector3 moveDirection;
    private Vector3 lastPlayerPosition;

    private bool isIdle;
    private bool isRunning;
    private bool isJumping;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        // Update player movement
        updateMovement();
        // Rotate player based on camera direction if moving
        updateRotation();
        // Update model animations
        updateAnimation();
    }

    void updateMovement() {
        // Get direction vectors
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float jumpVec = moveDirection.y;
        moveDirection = (transform.forward * vertical) + (transform.right * horizontal);
        // Normalize vector if there is diagnonal movement
        if (horizontal != 0 && vertical != 0) {
            moveDirection = moveDirection.normalized;
        }
        // Apply run only if player is walking
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            isIdle = false;
            if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isGrounded) {
                isRunning = !isRunning;
            }
        }
        else {
            isIdle = true;
            isRunning = false;
        }
        // Change speed if player is running
        if (isRunning) {
            moveDirection *= runningSpeed;
            if (isJumping) {
                moveDirection *= jumpRunMultiplier;
            }
        }
        else {
            moveDirection *= walkSpeed;
        }
        // No y velocity when player is grounded & handle jump event
        moveDirection.y = jumpVec;
        if (controller.isGrounded) {
            isJumping = false;
            moveDirection.y = 0;
            if (Input.GetButtonDown("Jump")) {
                isIdle = false;
                isJumping = true;
                moveDirection.y = jumpHeight;
            }
        }
        // Apply gravity and move player based on inputs
        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    // Method for updating player rotation
    void updateRotation() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Method for updating model animations
    void updateAnimation() {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", controller.isGrounded);
    }

}
