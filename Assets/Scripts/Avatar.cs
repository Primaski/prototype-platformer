using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Avatar : MonoBehaviour{

    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform ceilingChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Collider2D standingCollider;
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] public float speed = 1.0f;
    [SerializeField] public float runSpeedMultiplier = 1.5f;
    [SerializeField] public float crouchSpeedMultiplier = 0.75f;
    [SerializeField] public float bounciness = 1.0f;
    public float size = 4.0f;
    [SerializeField] private float groundCheckerRoomForError = 0.0001f;
    [SerializeField] private float ceilingCheckerRoomForError = 0.0001f;

    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isCrouching = false;
    private bool flipped = false;
    private float hVal, vVal;
    private bool crouchReleasedUnderSurface = false;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    
    void Start(){
        
    }

    void Update(){
        
        hVal = Input.GetAxis("Horizontal");
        if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && !isJumping) {
            isRunning = true;
        }else if(Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) {
            isRunning = false;
        }

        if (Input.GetButtonDown("Jump")) {
            if (!isJumping && isGrounded) {
                anim.SetBool("Jump", true);
                isJumping = true;
            }
        }else if (Input.GetButtonUp("Jump")) {
            isJumping = false;
        }

        if (Input.GetButtonDown("Crouch")) {
            if (isGrounded) isCrouching = true;
        } else if (Input.GetButtonUp("Crouch")) {
            isCrouching = false;
            if (CeilingCheck()) {
                crouchReleasedUnderSurface = true;
                isCrouching = true;
            }
        }

    }
    
    void FixedUpdate() {
        isGrounded = GroundCheck();
        Move(hVal); //controls both vertical and horizontal movement
    }

    void Move(float direction) {
        MoveHorizontal(direction);
        MoveVertical();
    }

    void MoveHorizontal(float direction) {

        if (crouchReleasedUnderSurface && !CeilingCheck()) { 
            isCrouching = false;
            crouchReleasedUnderSurface = false;
        }

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

    void MoveVertical() {
        if (rb.velocity.y < 0) isCrouching = false;
        if (isGrounded) {
            standingCollider.enabled = !isCrouching;
            if (isJumping && !isCrouching) {

                /* This one-liner was added as a band-aid to an issue whereby jumps would occur before the avatar was truly grounded, and so residual
                 * force downwards would result in very short jumps (replicate by holding down up jumping from a low elevation platform to a high one).
                 * By terminating downward force, all jumps will achieve equal height. If the collision issue were resolved, this could be safely removed. */
                rb.velocity = new Vector2(rb.velocity.x, 0);

                rb.AddForce(new Vector2(0f, bounciness * 350));
            }
            anim.SetBool("Crouch", isCrouching);
        }
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    bool GroundCheck() {
        bool grounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRoomForError, groundLayer);
        anim.SetBool("Jump", !grounded);
        return grounded;
    }

    bool CeilingCheck() {
        return Physics2D.OverlapCircle(ceilingChecker.position, ceilingCheckerRoomForError, groundLayer);
    }

}
