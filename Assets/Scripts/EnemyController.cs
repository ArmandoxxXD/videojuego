using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float attackRadius = 1.5f;
    public float speed = 2.0f;
    public Vector2 gizmoOffset = Vector2.zero;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    private bool isChasing = false;
    private bool isAttacking = false;
    private bool facingRight = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        Vector2 adjustedPosition = (Vector2)transform.position + gizmoOffset;
        float distanceToPlayer = Vector2.Distance(adjustedPosition, player.position);


        if (distanceToPlayer < attackRadius)
        {
            AttackPlayer();

        } else if (distanceToPlayer < detectionRadius)
        {
            isChasing = true;
            isAttacking = false;
            ChasePlayer();

        } else
        {
            isChasing = false;
            isAttacking = false;
            StopChase();
        }
        UpdateAnimations();
    }

    void ChasePlayer()
    {

        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
        movement = direction * speed;

        if ((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight))
        {
            Flip();
        }


        rb.velocity = new Vector2(movement.x, rb.velocity.y);
        Debug.Log("Detecto al jugador!");
    }

    void StopChase()
    {
        rb.velocity = Vector2.zero;
    }

    void AttackPlayer()
    {
        // Detener el movimiento al atacar
        rb.velocity = Vector2.zero;
        isAttacking = true;

        Debug.Log("Atacando al jugador!");
        // Implementar lógica de daño aquí si es necesario
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void UpdateAnimations()
    {
        float absVelocityX = Mathf.Abs(rb.velocity.x);

        // Si está persiguiendo, activar animación de caminar
        if (isChasing && !isAttacking)
        {
            animator.SetBool("isWalking", absVelocityX > 0); // Solo caminar si se está moviendo
        }
        else
        {
            animator.SetBool("isWalking", false); // Dejar de caminar
        }

        // Si está atacando, activar animación de ataque
        animator.SetBool("isAttacking", isAttacking);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 gizmoPosition = transform.position + (Vector3)gizmoOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gizmoPosition, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gizmoPosition, attackRadius);
    }

}
