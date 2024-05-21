using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //*Variable jump heights
    //*Higher gravity when falling
    //Apex time
    //*Coyote time
    //JumpBuffering
    //*MaxFallspeed



    #region Variables
    [Header("Variables")]
    [Tooltip("The movementspeed of the player")]
    [SerializeField] float movespeed;
    [Tooltip("The amount of force added to the players rigidbody.Y")]
    [Range(100f,500f)]
    [SerializeField] float jumpStrength;
    [SerializeField] bool isJumping;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    [SerializeField] LayerMask ground;
    [SerializeField] float coyoteTime = 0.25f;
    [SerializeField] float lastOnGroundTime;
    [SerializeField] float maxFallSpeed;
    #endregion

    #region Components
    private Rigidbody2D rb;
    #endregion

    #region Inputs
    private float hInput;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        #region TIMERS
        lastOnGroundTime -= Time.deltaTime;
        #endregion
        //Input
        hInput = Input.GetAxis("Horizontal");

        //Movement
        rb.velocity = new Vector2(movespeed * hInput, rb.velocity.y);


        //Jump
        if (Input.GetButtonDown("Jump") && CanJump())
        {
            Jump();
        }

        //Ground Check
        if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, ground)) //checks if set box overlaps with ground
        {
            lastOnGroundTime = coyoteTime; //if so sets the lastGrounded to coyoteTime
        }

        //Caps maximum fall speed
        if(rb.velocity.y > maxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x,maxFallSpeed);  


        //Adds a gravity multiplier while we have a negative Y velocity OR when we have a positive Y velocity and NOT inputing Jump.
        //Variable jump height && Higher gravity when falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Jump()
    {
        //Makes sure you cant get two jumps on quick inputs.
        lastOnGroundTime = 0;

        rb.AddForce(new Vector2(rb.velocity.x, jumpStrength));
    }

    private bool CanJump()
    {
        return lastOnGroundTime > 0.1f && !isJumping;
    }
}
