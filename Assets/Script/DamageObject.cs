using UnityEngine;

public class DamageObject : MonoBehaviour
{
    public int damageAmount = 10; // amount of damage to deal to the player
    public string targetTag = "Player"; // tag of the player object

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if the collision is with the player
        if (collision.gameObject.tag == targetTag)
        {
            // get the player's HP script
            Health playerHP = collision.gameObject.GetComponent<Health>();

            // if the player has an HP script, deal damage
            if (playerHP != null)
            {
                playerHP.TakeDamage(damageAmount);
            }
        }
    }
}