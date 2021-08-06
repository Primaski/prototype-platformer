using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AvatarMovement : MonoBehaviour{

    #region GlobalVariables
    [Header("Assets")]
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform ceilingChecker;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Collider2D standingCollider;
    private Rigidbody2D rb; //a
    private Animator anim;

    [Header("Modifiable Attributes")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float runSpeedMultiplier = 1.5f;
    [SerializeField] private float crouchSpeedMultiplier = 0.75f;
    [SerializeField] private float bounciness = 1.0f;
    [SerializeField] [Range(0, 10)] private int noOfJumps = 2;
    private float remainingJumps;
    private float size = 4.0f;

    [Header("Internal Variables")]
    private bool isRunning = false;
    private bool isGrounded = false;
    private bool isJumping = false;
    private bool isCrouching = false;
    private float hVal, vVal;
    private bool flipped = false;
    private bool crouchReleasedUnderSurface = false;
    private bool airborneJump = false; //if noOfJumps < 2, this boolean will be irrelevant
    private bool landingFlag = false; //set to true if just made contact with ground anew
    #endregion

    #region Startup and Updates
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        remainingJumps = noOfJumps;
    }

    private void Update(){
        
        hVal = Input.GetAxis("Horizontal");
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !isJumping) {
            isRunning = true;
        }else if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
            isRunning = false;
        }

        if (Input.GetButtonDown("Jump")) {
            Jump();
        }

        if (Input.GetButtonDown("Crouch")) {
            if (isGrounded && rb.velocity.y >= 0) isCrouching = true;
        } else if (Input.GetButtonUp("Crouch")) {
            if (!CeilingCheck()) { //no ceiling overhead, safe to uncrouch
                isCrouching = false;      
            } else { //ceiling overhead, stay crouching
                crouchReleasedUnderSurface = isCrouching = true;
            }
        }

    }

    private void FixedUpdate() {
        isGrounded = GroundCheck();
        if (crouchReleasedUnderSurface && !CeilingCheck()) {
            isCrouching = crouchReleasedUnderSurface = false; //reset back to normal
        }
        if (landingFlag) { 
            remainingJumps = noOfJumps;
            landingFlag = isJumping = false;
        }

        Move(hVal);
    }
    #endregion

    #region Movement

    private void Move(float direction) {
        Crouch(); //does not crouch automatically, sets necessary animations and colliders if conditions are met

        float speed = 
            (isRunning ? runSpeedMultiplier : 1) * 
            (isCrouching ? crouchSpeedMultiplier : 1) *
            this.speed * Time.fixedDeltaTime * 200;
        Vector2 targetVelocity = new Vector2(direction * speed, rb.velocity.y);
        rb.velocity = targetVelocity;

        Vector3 newScale = transform.localScale;

        if (!flipped && direction < 0) { //flip if moving the other way
            newScale.x = -size;
            flipped = true;
        } else if (flipped && direction > 0) {
            newScale.x = size;
            flipped = false;
        }

        transform.localScale = newScale;
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }

    private void Crouch() {
        if (isGrounded) {
            standingCollider.enabled = !isCrouching;
            anim.SetBool("Crouch", isCrouching);
        }
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void Jump() {
        if (remainingJumps > 0) {
            if (isGrounded) {
                isJumping = true;
                remainingJumps--;
                if (remainingJumps >= 1) airborneJump = true;
                rb.velocity = new Vector2(rb.velocity.x, bounciness * 7);
                anim.SetBool("Jump", true);
            } else {
                if (airborneJump) {
                    remainingJumps--;
                    rb.velocity = new Vector2(rb.velocity.x, bounciness * 7);
                    anim.SetBool("Jump", true);
                }
            }
        }
    }
    #endregion

    #region Miscellaneous
    private bool GroundCheck() {
        bool grounded = Physics2D.OverlapCircle(groundChecker.position, 0.05f, groundLayer);
        anim.SetBool("Jump", !grounded);

        if (grounded && !isGrounded) landingFlag = true; //state just changed, just landed on ground

        return grounded;
    }

    private bool CeilingCheck() {
        return Physics2D.OverlapCircle(ceilingChecker.position, 0.05f, groundLayer);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundChecker.position, 0.05f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(ceilingChecker.position, 0.05f);
    }
    #endregion

}
