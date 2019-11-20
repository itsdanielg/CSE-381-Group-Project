using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public CharacterController controller;
    public Animator animator;
    public GameObject playerModel;
    public Transform lookAt;

    public GameObject bullet;
    public Camera camera;
    public GameObject crosshair;

    public float walkSpeed;
    public float runningSpeed;
    public float speedMultiplier;
    public float jumpHeight;
    public float gravity;
    public float rotationSpeed;
    public float hookRange;
    public float hookTravelSpeed = 8.0f;
    public float hookSensitivity = 0.5f;

    private Vector3 moveDirection;
    private RaycastHit hit;

    private bool isIdle;
    private bool isRunning;
    private bool isJumping;
    private bool isLongJumping;
    private bool isRolling;
    private bool isReeling;
    private float rollTime;
    private float rollDuration;
    private float finalSpeed;

    private bool hookPlaced;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        initVar();

        crosshair.SetActive(false);
        bullet.SetActive(false);
        hookPlaced = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (crosshair.activeSelf) {
                shootEvent();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            crosshair.SetActive(!crosshair.activeSelf);
            
        }
        if (Input.mouseScrollDelta.y < - 1 && hookPlaced) {
            isReeling = true;
        }
        // Check if player is already reeling
        if (isReeling) {
            moveDirection = new Vector3(0, 0, 0);
            controller.Move(moveDirection);
            reelEvent();
        }
        else {
            // Update player movement
            updateMovement();
            // Rotate player based on camera direction if moving
            updateRotation();
        }
        
        // Update model animations
        updateAnimation();
    }

    void initVar() {
        isIdle = true;
        isRunning = false;
        isJumping = false;
        isLongJumping = false;
        isRolling = false;
        isReeling = false;
        rollTime = 0;
        rollDuration = 1.0f;
        finalSpeed = 0;
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
        // Apply certain key presses only when character is walking
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            isIdle = false;
            // Run/disable run only when character is grounded
            if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isGrounded) {
                isRunning = !isRunning;
            }
            // Initiate roll only if character is grounded and not already rolling
            if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded && !isRolling) {
                isRolling = true;
            }
        }
        else {
            isIdle = true;
            isRunning = false;
            isRolling = false;
        }
        // Change speed if player is running
        if (isRunning) {
            finalSpeed = runningSpeed;
        }
        else {
            finalSpeed = walkSpeed;
        }
        moveDirection *= finalSpeed;
        // Event for character rolling
        if (isRolling) {
            if (rollTime != 0) {
                controller.height = 1.0f;
                controller.center = new Vector3(0, -0.5f, 0);
            }
            rollTime += Time.deltaTime;
            if (rollTime > rollDuration) {
                controller.height = 2.0f;
                controller.center = new Vector3(0, 0, 0);
                isRolling = false;
                rollTime = 0;
            }
        }
        else {
            controller.height = 2.0f;
            controller.center = new Vector3(0, 0, 0);
            rollTime = 0;
        }
        // No y velocity when player is grounded & handle jump event
        moveDirection.y = jumpVec;
        if (controller.isGrounded) {
            isJumping = false;
            isLongJumping = false;
            moveDirection.y = 0;
            if (Input.GetButton("Jump")) {
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
            transform.rotation = Quaternion.Euler(0f, lookAt.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Method for updating model animations
    void updateAnimation() {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetBool("isRolling", isRolling);
    }

    void shootEvent() {
        if (!isReeling) {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, hookRange)) {
                hookPlaced = true;
                bullet.SetActive(true);
                bullet.transform.position = hit.point;
            }
        }
    }

    void reelEvent() {
        //controller.Move(new Vector3(0, 0, 0));
        //controller.transform.position = hit.point;
        float step =  hookTravelSpeed * Time.deltaTime;
        float distance = Vector3.Distance(controller.transform.position, bullet.transform.position);
        if (distance > 1) {
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, bullet.transform.position, step);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(bullet.transform.position.x, 0f, bullet.transform.position.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotationSpeed * Time.deltaTime * 5);
        }
        else {
            hookPlaced = false;
            isReeling = false;
            bullet.SetActive(false);
        }
    }

}
