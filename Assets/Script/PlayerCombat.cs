using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    public Transform groundCheck; // Add this line
    public float groundCheckRadius; // Add this line
    public LayerMask whatIsGround; // Add this line
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    private float timeSinceLastAttack = 0;
    public float timeBetweenAttacks = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastAttack >= timeBetweenAttacks && Input.GetMouseButtonDown(0) && IsGrounded())
        {
            Attack();
            timeSinceLastAttack = 0;
        }
        else
        {
            timeSinceLastAttack += Time.deltaTime;
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}