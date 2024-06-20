using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth {  get; private set; }

    private Animator anim;
    private bool dead;

    public static event Action OnPlayerDead;

    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("IsHurt");
        }
        else
        {
            if(!dead) 
            {
                anim.SetTrigger("IsDead");
                GetComponent<PlayerController>().enabled = false;
                GetComponent<PlayerCombat>().enabled = false;
                dead = true;
                OnPlayerDead?.Invoke();
            }
            
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}
