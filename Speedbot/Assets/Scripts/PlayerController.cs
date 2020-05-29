using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private const float WALK_ANIMATION_SPEED = 1.5f;
    private const float RUN_ANIMATION_SPEED = 2.0f;
    private const float ROLL_MAX_DURATION = 0.5f;
    private const float ROTATION_SPEED = 20.0f;
    private const float WALK_SPEED = 4.0f;
    private const float RUN_SPEED = 8.0f;
    private const float CROSSHAIR_SPEED_MULTIPLIER = 0.7f;
    private const float ROLL_SPEED_MULTIPLIER = 1.4f;
    private const float JUMP_HEIGHT = 20.0f;
    private const float DOUBLE_JUMP_HEIGHT_MULTIPLIER = 0.8f;
    private const float GRAVITY = -40.0f;
    private const float HOOK_SENSITIVITY = 1.0f;
    private const float HOOK_TRAVEL_SPEED = 40.0f;
    private const int MAX_JUMPS = 2;

    public static CharacterController controller;
    public GameObject playerModel;
    public Animator animator;
    public Camera camera;
    public Transform lookAt;
    public GameObject crosshair;
    public GameObject bullet;
    public LineRenderer laser;
    public AudioSource shootSound;
    public AudioSource reelSound;
    public AudioSource deathSound;

    public static int currentJump;
    public static float boostMultiplier = 1.0f;
    public static float hookRange = 70;
    
    public static Vector3 respawnPoint;
    private static Vector3 moveDirection;
    private RaycastHit hit;

    private bool isIdle;
    private bool isRunning;
    private bool isJumping;
    private bool isRolling;
    private bool isReeling;
    public static bool hookPlaced;
    private float finalSpeed;
    private float rollCurrentDuration;

    ////////////////////////////////////////////////// START //////////////////////////////////////////////////
    void Start() {
        controller = GetComponent<CharacterController>();
        crosshair.SetActive(false);
        bullet.SetActive(false);
        initVar();
    }

    ////////////////////////////////////////////////// UDPATE //////////////////////////////////////////////////
    void Update() {

        if (OptionInputs.gamePaused) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // GET LEFT MOUSE CLICK EVENT
        if (Input.GetMouseButtonDown(0)) {
            shootEvent();
        }
        // GET RIGHT MOUSE CLICK EVENT
        if (Input.GetMouseButtonDown(1)) {
            crosshair.SetActive(!crosshair.activeSelf);
        }
        // GET MOUSE WHEEL EVENT ONLY IF HOOK IS PLACED AND WHEEL IS SCROLLED QUICKLY
        if ((Input.mouseScrollDelta.y < -HOOK_SENSITIVITY || Input.mouseScrollDelta.y > HOOK_SENSITIVITY) && hookPlaced) {
            reelSound.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
            reelSound.Play();
            isReeling = true;
        }
        // THEN CONTINOUSLY UPDATE LASER
        updateLaser();
        // AND CONTINOUSLY UPDATE CROSSHAIR
        updateCrosshair();

        // SET REEL EVENT AND DISABLE USER INPUT IF PLAYER IS CURRENTLY REELING
        if (isReeling) {
            moveDirection = new Vector3(0, 0, 0);
            controller.Move(moveDirection);
            reelEvent();
        }
        // ELSE LISTEN TO USER INPUT
        else {
            updateMovement();       // UPDATE PLAYER MOVEMENT
            updateRotation();       // ROTATE MOVING PLAYER BASED ON CAMERA DIRECTION
        }

        // MANUALLY RESPAWN
        if (Input.GetKeyDown("r")) {
            deathSound.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
            deathSound.Play();
            respawnPlayer(respawnPoint);
        }

        // LASTLY UPDATE MODEL ANIMATIONS
        updateAnimation();

    }

    void OnTriggerEnter(Collider trigger) {
        if (trigger.CompareTag("TriggerZone")) {
            trigger.gameObject.SetActive(false);
        }
    }

    ////////////////////////////////////////////////// INIT VAR //////////////////////////////////////////////////
    // INITATE PRIVATE VARIABLES TO BE USED
    void initVar() {
        isIdle = true;
        isRunning = false;
        isJumping = false;
        isRolling = false;
        isReeling = false;
        hookPlaced = false;
        finalSpeed = 0;
        rollCurrentDuration = 0;
        currentJump = 0;
    }

    ////////////////////////////////////////////////// UDPATE MOVEMENT //////////////////////////////////////////////////
    // UPDATE MOVEMENT INPUTS
    void updateMovement() {

        // GET DIRECTION VECTORS (FROM WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float jumpVec = moveDirection.y;
        moveDirection = (transform.forward * vertical) + (transform.right * horizontal);
        // NORMALIZE VECTOR IF THERE IS DIAGONAL MOVEMENT
        if (horizontal != 0 && vertical != 0) {
            moveDirection = moveDirection.normalized;
        }

        // APPLY CERTAIN ACTIONS ONLY WHEN CHARACTER IS MOVING
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            isIdle = false;
            // Run/disable run only when character is grounded and not rolling
            if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isGrounded && !isRolling) {
                isRunning = !isRunning;
            }
            // Initiate roll only if character is grounded and not already rolling
            if (Input.GetKeyDown(KeyCode.LeftControl) && controller.isGrounded && !isRolling) {
                isRolling = true;
            }
        }
        // ELSE CHARACTER IS IDLE
        else {
            isIdle = true;
            isRunning = false;
            isRolling = false;
        }

        // CHANGE SPEED IF PLAYER IS RUNNING OR WALKING
        if (isRunning) {
            finalSpeed = RUN_SPEED * boostMultiplier;
            animator.SetFloat("runSpeed", RUN_ANIMATION_SPEED * boostMultiplier);
        }
        else {
            finalSpeed = WALK_SPEED * boostMultiplier;
            animator.SetFloat("walkSpeed", WALK_ANIMATION_SPEED * boostMultiplier);
        }
        // SLOW DOWN CHRACTER IF CROSSHAIR IS TOGGLED ON
        if (crosshair.activeSelf) {
            finalSpeed *= CROSSHAIR_SPEED_MULTIPLIER * boostMultiplier;
            animator.SetFloat("runSpeed", RUN_ANIMATION_SPEED * CROSSHAIR_SPEED_MULTIPLIER * boostMultiplier);
            animator.SetFloat("walkSpeed", WALK_ANIMATION_SPEED * CROSSHAIR_SPEED_MULTIPLIER * boostMultiplier);
        }

        // DURATION BASED EVENT FOR CHARACTER ROLLING
        if (isRolling) {
            controller.height = 1.0f;                                       // LOWER PLAYER HEIGHT WHEN ROLLING
            controller.center = new Vector3(0, -0.5f, 0);
            finalSpeed *= ROLL_SPEED_MULTIPLIER;
            rollCurrentDuration += Time.deltaTime;
            if (rollCurrentDuration >= ROLL_MAX_DURATION) {
                controller.height = 2.0f;                                   // RESET PLAYER HEIGHT AFTER ROLLING
                controller.center = Vector3.zero;
                isRolling = false;
                rollCurrentDuration = 0;
            }
            animator.SetFloat("rollSpeed", 1.0f/ROLL_MAX_DURATION);         // CHANGE ANIMATION SPEED BASED ON ROLL DURATION
        }

        // UPDATE FINAL CHARACTER SPEED ON HORIZONTAL PLANE
        moveDirection *= finalSpeed;

        // RESET Y VELOCITY IF CHARACTER IS GROUNDED
        moveDirection.y = jumpVec;
        if (controller.isGrounded) {
            isJumping = false;
            moveDirection.y = 0;
            currentJump = 0;
        }

        // LISTEN FOR JUMP AND DOUBLE JUMP INPUTS
        if (Input.GetButtonDown("Jump")) {
            // ONLY JUMP IF PLAYER HAS NOT EXCEED MAX JUMPS
            if (currentJump < MAX_JUMPS) {
                // PROCESS DOUBLE JUMP ONLY IF PLAYER IS MID-AIR
                if (!controller.isGrounded) {
                    currentJump = 2;
                    animator.Play("Double Jump");
                    moveDirection.y = JUMP_HEIGHT * DOUBLE_JUMP_HEIGHT_MULTIPLIER;
                }
                // ELSE PROCESS REGULAR JUMP
                else {
                    currentJump = 1;
                    isIdle = false;
                    isJumping = true;
                    moveDirection.y = JUMP_HEIGHT;
                }
            }
        }

        // FINALLY, APPLY GRAVITY AND MOVE PLAYER BASED ON INPUTS
        moveDirection.y += GRAVITY * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    ////////////////////////////////////////////////// UDPATE ROTATION //////////////////////////////////////////////////
    // UPDATE PLAYER MODEL ROTATION
    void updateRotation() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            transform.rotation = Quaternion.Euler(0f, lookAt.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, ROTATION_SPEED * Time.deltaTime);
        }
    }

    ////////////////////////////////////////////////// UDPATE ANIMATION //////////////////////////////////////////////////
    // UPDATE PLAYER MODEL ANIMATIONS
    void updateAnimation() {
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetBool("isRolling", isRolling);
    }
    
    ////////////////////////////////////////////////// UDPATE CROSSHAIR //////////////////////////////////////////////////
    // UPDATE CROSSHAIR COLOR WHEN TARGET IS IN RANGE
    void updateCrosshair() {
        if (crosshair.activeSelf) {
            Image[] children = crosshair.GetComponentsInChildren<Image>();
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, hookRange)) {
                    if (hit.transform.tag == "Building") {
                        for (int i = 0; i < 4; i++) {
                            children[i].color = Color.green;
                        }
                    }
                    else {
                        for (int i = 0; i < 4; i++) {
                            children[i].color = Color.red;
                        }
                    }
                }
                else {
                    for (int i = 0; i < 4; i++) {
                        children[i].color = Color.red;
                    }
                }
        }
    }

    ////////////////////////////////////////////////// SHOOT EVENT //////////////////////////////////////////////////
    // SHOOT TARGET IF AND ONLY IF PLAYER IS NOT ROLLING
    void shootEvent() {
        if (crosshair.activeSelf) {
            if (!isReeling) {
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, hookRange)) {
                    if (hit.transform.tag == "Building") {
                        shootSound.volume = PlayerPrefs.GetFloat("Sound")/100.0f;
                        shootSound.Play();
                        hookPlaced = true;
                        bullet.SetActive(true);
                        bullet.transform.position = hit.point;
                        laser = laser.GetComponent<LineRenderer>();
                        laser.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    ////////////////////////////////////////////////// REEL EVENT //////////////////////////////////////////////////
    // REEL PLAYER ONTO TARGET PLACED
    void reelEvent() {
        float step =  HOOK_TRAVEL_SPEED * Time.deltaTime;
        float distance = Vector3.Distance(controller.transform.position, bullet.transform.position);
        if (distance > 1) {
            controller.transform.position = Vector3.MoveTowards(controller.transform.position, bullet.transform.position, step);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(bullet.transform.position.x, 0f, bullet.transform.position.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, ROTATION_SPEED * Time.deltaTime * 5);
            animator.Play("Reel");
            isIdle = false;
            isJumping = false;
            isRolling = false;
        }
        else {
            reelSound.Stop();
            hookPlaced = false;
            isReeling = false;
            bullet.SetActive(false);
            laser.gameObject.SetActive(false);
        }
    }

    void updateLaser() {
        if (hookPlaced) {
            laser.SetPosition(0, controller.transform.position);
            laser.SetPosition(1, bullet.transform.position);
        }
    }

    public static void respawnPlayer(Vector3 respawnPos) {
        moveDirection = Vector3.zero;
        controller.transform.position = respawnPos;
    }

    public static void respawnPlayer() {
        moveDirection = Vector3.zero;
        controller.transform.position = respawnPoint;
    }

}
