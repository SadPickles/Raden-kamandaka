using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private Vector2 detectionBoxSize;
    [SerializeField] private LayerMask playerLayer;
    public Transform Player { get; set; }

    private bool isPlayerDetected;
    private SpriteRenderer enemySpriteRenderer;
    private Vector3 originalScale;

    // Start is called before the first frame update
    private void Awake()
    {
        isPlayerDetected = false;
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, detectionBoxSize, 0, playerLayer);

        if (hitColliders.Length > 0)
        {
            isPlayerDetected = true;
            Player = hitColliders[0].transform;

            // Flip the sprite based on the player's position
            if (Player.position.x > transform.position.x)
            {
                enemySpriteRenderer.flipX = true;
                transform.localScale = new Vector3(originalScale.x * -1, originalScale.y, originalScale.z);
            }
            else
            {
                enemySpriteRenderer.flipX = false;
                transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            }
        }
        else
        {
            isPlayerDetected = false;
            Player = null;
        }
    }

    public bool IsPlayerDetected()
    {
        return isPlayerDetected;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }
}