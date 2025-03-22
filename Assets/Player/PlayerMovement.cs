using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator anim;
    private Collider playerCollider;
    public bool isPlayer = true; // Default is player
    public float groundY;
    [Header("Movement")]
    public float moveSpeed;
    public float maxSpeed = 15f;
    public float airControl = 0.5f;
    public float airDrag = 1f;
    public float groundDrag;

    [Header("Jump & Height Clamp")]
    public float maxHeight = 12f;
    private bool disableClamp = false; 
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }
        rb.linearVelocity = Vector3.zero;

        anim = GetComponentInChildren<Animator>();
        groundY = transform.position.y; // Store starting height

        // Get the Collider from the player object (child of the empty GameObject)
        playerCollider = GetComponentInChildren<Collider>();
        
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (playerCollider != null)
        {
            grounded = Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + 0.1f, whatIsGround);
        }
        else
        {
            Debug.LogError("Player Collider not found!");
        }
        
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            StartCoroutine(ThrowAnimation(false)); // Normal throw
        }
        if (Input.GetMouseButtonDown(1)) // Right Click
        {
            StartCoroutine(ThrowAnimation(true)); // Right-click special throw
        }

        if (transform.position.y < groundY + -1f)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
            MyInput();
            ApplyDrag();
            SpeedControl();
            ClampHeight();

            UpdateAnimations();
            // handle drag
            if (grounded)
                Debug.Log("Grounded");
            else
                Debug.Log("Airborne");
    }
    void FixedUpdate()
    {
        MovePlayer();
        LimitSpeed(); //  Ensure speed does not exceed maxSpeed
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

     private void MovePlayer()
    {
        // calculate movement direction
        // moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        // rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);

        // Calculate movement direction
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        // Apply different movement in air vs ground
        if (grounded)
        {
            rb.AddForce(moveSpeed * moveDirection, ForceMode.Force);
        }
        else
        {
            // Only apply air control to horizontal movement, keep vertical (Y) unaffected
            Vector3 airMove = moveSpeed * airControl * new Vector3(moveDirection.x, 0f, moveDirection.z);
            rb.AddForce(airMove, ForceMode.Force);
        }
    }
 
    private void ApplyDrag()
    {
        // Apply ground or air drag...get horizontal velocity only 
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // apply drag only to horizontal movement 
        float currentDrag = grounded ? groundDrag : airDrag;
        horizontalVelocity *= 1f - (currentDrag * Time.deltaTime);

        // Preserve vertical velocity (gravity should remain strong)
        rb.linearVelocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
    }
     private void SpeedControl()
    {
        // Get current horizontal speed
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // If moving too fast, clamp speed
        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    private void ClampHeight()
    {
        if (!disableClamp && rb.linearVelocity.y > maxHeight)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, maxHeight, rb.linearVelocity.z);
        }
    }

       private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("LaunchPad"))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 20f, rb.linearVelocity.z);
        }
    }
     private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Ignore Y (jumping)

        if (flatVelocity.magnitude > maxSpeed)
        {
            // Clamp velocity to max speed
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    public void DisableClamp(bool disable)
    {
        disableClamp = disable;
    }
 
    private void UpdateAnimations()
    {
        //Get horizontal movemnt speed
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        // set animation [parameters]
        anim.SetFloat("Speed", horizontalSpeed); // speed affects running and idle transition
        anim.SetBool("Jump", !grounded);// jump is true if not grounded
        anim.SetBool("IsGrounded", grounded); // helps return from jump
    }

    IEnumerator ThrowAnimation(bool isRightClick)
{
    if (isRightClick)
    {
        anim.SetBool("isRightThrow", true); // Right-click throw animation
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isRightThrow", false);
    }
    else
    {
        anim.SetBool("isThrow", true); // Left-click throw animation
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isThrow", false);
    }
}

}