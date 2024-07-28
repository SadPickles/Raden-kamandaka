using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRate;
    private float nextAttackTime;

    public float knockbackForce = 5.0f;

    public bool isAttacking;

    [SerializeField] private Health playerHealth;

    private Animator anim;

    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerHealth.currentHealth > 0 && IsPlayerInAttackRange())
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                Attack();
                isAttacking = true;
            }
        }
        else
        {
            isAttacking = false;
        }
    }

    private bool IsPlayerInAttackRange()
    {
        float distance = Vector3.Distance(transform.position, playerHealth.transform.position);
        return distance <= attackRange;
    }

    private void Attack()
    {
        anim.SetTrigger("IsAttacking");
        playerHealth.TakeDamage(attackDamage);

        // Get the player's Rigidbody2D
        Rigidbody2D playerRigidbody = playerHealth.GetComponent<Rigidbody2D>();

        // Apply a force to the player's Rigidbody2D
        playerRigidbody.AddForce(new Vector2(knockbackForce * Mathf.Sign(transform.position.x - playerHealth.transform.position.x), knockbackForce), ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}