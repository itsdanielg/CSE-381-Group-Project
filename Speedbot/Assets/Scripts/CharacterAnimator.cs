using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour {

    Animator animator;
    CharacterController controller;

    private bool isRunning; 
    public float walkSpeed = 3.0f;
    public float sprintSpeed = 5.0f;

    // Start is called before the first frame update
    void Start() {
        animator = gameObject.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        float finalSpeed = 0;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (Input.GetKey("w")) {
            if (isRunning) {
                finalSpeed = sprintSpeed;
                animator.SetInteger("animParam", 3);
            }
            else {
                finalSpeed = walkSpeed;
                animator.SetInteger("animParam", 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                if (isRunning) {
                    animator.SetInteger("animParam", 2);
                }
                isRunning = !isRunning;
            }
            if (Input.GetKey("d")) {
                movement = movement.normalized;
            }
            else if (Input.GetKey("a")) {
                movement = movement.normalized;
            }
        }
        else {
            finalSpeed = walkSpeed;
            if (isRunning) {
                animator.SetInteger("animParam", 5);
            }
            else {
                animator.SetInteger("animParam", 0);
            }
            isRunning = false;
        }
        Vector3 playerMovement =  movement * finalSpeed * Time.deltaTime;
        transform.Translate(playerMovement, Space.Self);
    }
}
