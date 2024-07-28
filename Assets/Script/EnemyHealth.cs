using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }
    public Animator animator; // Add an Animator variable
    public PatrolState patrolState;
    public EnemyCombat enemyCombat;
    [SerializeField] private FloatingHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar((float)currentHealth, (float)maxHealth);
        animator = GetComponent<Animator>();

        // Get the PatrolState component
        patrolState = GetComponent<PatrolState>();
        enemyCombat = GetComponent<EnemyCombat>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar((float)currentHealth, (float)maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Hurt(); // Call the Hurt function
        }
    }

    void Die()
    {
        // Add death behavior here, such as destroying the enemy object
        patrolState.enabled = false;
        enemyCombat.enabled = false;
        animator.SetTrigger("Die"); // Trigger the "Die" animation
        Destroy(gameObject, 1.0f); // Destroy the enemy object after 1 second
    }

    void Hurt()
    {
        animator.SetTrigger("Hurt"); // Trigger the "Hurt" animation
    }
}