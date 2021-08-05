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
            isJumping = true;
        }else if (Input.GetButtonUp("Jump")) {
            isJumping = false;
        }

        if (Input.GetButtonDown("Crouch")) {
            isCrouching = true;
        } else if (Input.GetButtonUp("Crouch")) {
            isCrouching = false;
            if (CeilingCheck()) {
                crouchReleasedUnderSurface = true;
                isCrouching = true;
            }
        }
    }
    
    void FixedUpdate() {
        Move(hVal);
        isGrounded = GroundCheck();
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
        UnityEngine.Debug.Log(Mathf.Abs(rb.velocity.x));
    }

    void MoveVertical() {
        if (isGrounded) {
            standingCollider.enabled = !isCrouching;
            if (isJumping && !isCrouching) {
                rb.AddForce(new Vector2(0f, bounciness * 175));
            }
            anim.SetBool("Crouch", isCrouching);
        }
    }

    bool GroundCheck() {
        //return Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundLayer);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundChecker.position, 0.1f, groundLayer);
        isGrounded = (colliders.Length > 0) ? true : false;
        return isGrounded;
    }

    bool CeilingCheck() {
        return Physics2D.OverlapCircle(ceilingChecker.position, 0.1f, groundLayer);
    }

}
