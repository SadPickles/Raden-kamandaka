using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    private int currentPoint = 0; // Index of the current patrol point
    public float speed = 3f; // Movement speed of the enemy
    public float idleTime = 1f; // Time to wait before switching to the next patrol point
    public float groundCheckDistance = 0.5f; // Distance to check for ground
    public LayerMask groundLayer; // Layer mask for ground objects
    private Animator animator; // Reference to the animator component

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the animator component
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the enemy is standing on ground
        if (IsGrounded())
        {
            if (transform.position != patrolPoints[currentPoint].position)
            {
                // Move the enemy towards the current patrol point
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].position, speed * Time.deltaTime);

                // Set the walk animation
                animator.SetBool("IsWalking", true);

                // Flip the sprite based on the direction of movement
                if (transform.position.x < patrolPoints[currentPoint].position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                // Set the idle animation
                animator.SetBool("IsWalking", false);

                // Start the coroutine to wait for the idle time
                StartCoroutine(WaitForIdleTime());
            }
        }
        else
        {
            // Set the idle animation
            animator.SetBool("IsWalking", false);
        }
    }

    // Function to check if the enemy is standing on ground
    bool IsGrounded()
    {
        // Cast a ray downwards from the enemy's position to check for ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // Return true if the raycast hits a ground object
        return hit.collider != null;
    }

    // Coroutine to wait for the idle time before switching to the next patrol point
    IEnumerator WaitForIdleTime()
    {
        yield return new WaitForSeconds(idleTime);

        // Switch to the next patrol point
        currentPoint++;

        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
        }
    }

    // Function to draw a gizmo for the ground check
    void OnDrawGizmos()
    {
        // Draw a ray for the ground check
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}