using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    [Header("Chase Parameters")]
    [SerializeField] private Vector2 chaseRange;
    [SerializeField] private float stopChaseRangeRadius;
    [SerializeField] private Transform player;

    public float StopChaseRangeRadius { get; private set; }

    private EnemyCombat enemyCombat;
    private bool isIdling = false;

    private IEnumerator IdleRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleDuration);

            if (!enemyCombat.isAttacking && !isIdling)
            {
                isIdling = true;
                DirectionChange();
            }
        }
    }


    private void Start()
    {
        enemyCombat = GetComponent<EnemyCombat>();
        player = GameObject.FindWithTag("Player").transform;

        // Set the stop chase range radius to 50% of the chase range radius
        StopChaseRangeRadius = ChaseRangeRadius * 0.5f;
        StartCoroutine(IdleRoutine());
    }

    private void Awake()
    {
        initScale = enemy.localScale;
    }
    private void OnDisable()
    {
        anim.SetBool("IsWalking", false);
    }

    private void Update()
    {
        if (!enemyCombat.isAttacking)
        {
            float distanceToPlayer = Vector3.Distance(player.position, enemy.position);
            if (distanceToPlayer <= ChaseRangeRadius)
            {
                // Chase the player
                isIdling = false;
                ChaseState chaseState = new ChaseState();
                chaseState.Execute(this);
            }
            else
            {
                // Patrol
                if (movingLeft)
                {
                    if (enemy.position.x >= leftEdge.position.x)
                        MoveInDirection(-1);
                    else
                        DirectionChange();
                }
                else
                {
                    if (enemy.position.x <= rightEdge.position.x)
                        MoveInDirection(1);
                    else
                        DirectionChange();
                }
            }
        }
        else
        {
            // Stop patrol state
            isIdling = false;
        }
    }


    public class ChaseState
    {
        public void Execute(PatrolState patrolState)
        {
            // Move the enemy towards the player
            Vector3 directionToPlayer = (patrolState.player.position - patrolState.enemy.position).normalized;
            patrolState.enemy.position += directionToPlayer * patrolState.speed * Time.deltaTime;

            // Make the enemy face the player
            patrolState.enemy.localScale = new Vector3(Mathf.Sign(patrolState.player.position.x - patrolState.enemy.position.x) * Mathf.Abs(patrolState.initScale.x),
                patrolState.initScale.y, patrolState.initScale.z);

            // Set the enemy's animator to chase
            patrolState.anim.SetBool("IsWalking", true);

            // Check if the player is within the stop chase range
            if (Vector3.Distance(patrolState.enemy.position, patrolState.player.position) > patrolState.StopChaseRangeRadius)
            {
                // Stop chasing
                patrolState.anim.SetBool("IsWalking", true);
            }
            else
            {
                // Stop chasing
                patrolState.anim.SetBool("IsWalking", false);
                patrolState.enemyCombat.isAttacking = true;
            }
        }
    }

    private void DirectionChange()
    {
        anim.SetBool("IsWalking", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            isIdling = false;
        }
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("IsWalking", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.position, ChaseRangeRadius * 0.9f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(enemy.position, StopChaseRangeRadius * 0.9f);
    }

    public float ChaseRangeRadius
    {
        get { return chaseRange.magnitude; }
    }
}