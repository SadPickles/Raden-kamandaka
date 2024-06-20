using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float horizontalInput;
    public float moveSpeed = 5f;
    bool isFacingRight = true;
    public float jumpPower = 5f;
    public float jumpTime = 0.5f; // Jump time
    public float fallMultiplier = 2.5f; // Fall multiplier
    public float jumpMultiplier = 1.5f; // Jump multiplier
    bool isGrounded = false;
    private Vector2 targetVelocity;

    public AudioClip runningAudioClip; // Add this line
    AudioSource audioSource;

    public Rigidbody2D rb;
    public Animator animator;
    float jumpTimer = 0f; // Jump timer
    public Transform groundCheck; // Add this line
    public float groundCheckRadius; // Add this line
    public LayerMask whatIsGround; // Add this line

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        groundCheck = GameObject.Find("GroundCheck").transform; // Add this line
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (GetComponent<EnergySystem>().currentEnergy > 0)
            {
                moveSpeed = 10f; // increase move speed when Shift is pressed
            }
        }
        else
        {
            moveSpeed = 5f; // normal move speed
        }

        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0 && !audioSource.isPlaying)
        {
            audioSource.clip = runningAudioClip;
            audioSource.Play();
        }
        else if (horizontalInput == 0 && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        horizontalInput = Input.GetAxis("Horizontal");

        FlipSprite();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
            jumpTimer = jumpTime; // Reset jump timer
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (Input.GetButton("Jump") && jumpTimer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower * jumpMultiplier);
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && !animator.GetBool("isJumping") && isGrounded == false)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        if (rb.velocity.y < 0 && !animator.GetBool("isJumping"))
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnTriggerEnter2D(Collider2D colission)
    {
        if (colission.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
            jumpTimer = 0f; // Reset jump timer when landing
        }
    }

    public bool IsMoving()
    {
        return horizontalInput != 0f;
    }
}