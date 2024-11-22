using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float patrolSpeed = 2f; // Velocidad de patrullaje
    public float chaseSpeed = 3f; // Velocidad al perseguir (ajustada para que sea más lenta)
    public float detectionRange = 5f; // Rango de detección del jugador
    public float attackRange = 1.5f; // Rango de ataque
    public float patrolDistance = 3f; // Distancia que patrulla el enemigo
    public LayerMask groundLayer; // Para evitar atravesar el suelo

    private bool isChasing = false; // Si está persiguiendo al jugador
    private bool isAttacking = false; // Si está atacando
    private Vector3 patrolStartPosition; // Punto inicial para patrullar
    private Vector3 patrolTargetPosition; // Posición a la que patrullar
    private bool movingRight = true; // Dirección de patrullaje

    private Rigidbody2D rb; // Referencia al Rigidbody2D
    private Animator animator; // Referencia al Animator

    void Start()
    {
        // Inicializa el Rigidbody2D y el Animator
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Guardamos la posición inicial para patrullar alrededor de este punto
        patrolStartPosition = transform.position;
        patrolTargetPosition = patrolStartPosition + new Vector3(patrolDistance, 0, 0); // Patrulla hacia la derecha
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de detección pero fuera del rango de ataque, perseguir
        if (distanceToPlayer < detectionRange && distanceToPlayer > attackRange)
        {
            isChasing = true;
            isAttacking = false;
        }
        // Si el jugador está fuera del rango de detección, dejar de perseguir y seguir patrullando
        else if (distanceToPlayer > detectionRange)
        {
            isChasing = false;
            isAttacking = false;
        }

        // Si el jugador está dentro del rango de ataque
        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
        }

        if (isAttacking)
        {
            AttackPlayer();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);

        // Movimiento de patrullaje usando Rigidbody2D para evitar problemas con colisiones
        if (movingRight)
        {
            rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            if (Vector2.Distance(transform.position, patrolTargetPosition) < 0.1f)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
            if (Vector2.Distance(transform.position, patrolStartPosition) < 0.1f)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        // Perseguir al jugador con una velocidad menor para evitar que se mantenga siempre pegado
        float step = chaseSpeed * Time.deltaTime;
        Vector2 targetPosition = new Vector2(player.position.x, rb.position.y);
        rb.position = Vector2.MoveTowards(rb.position, targetPosition, step);

        // Voltear hacia el jugador
        if ((player.position.x > transform.position.x && !movingRight) || (player.position.x < transform.position.x && movingRight))
        {
            Flip();
        }
    }

    void AttackPlayer()
    {
        // Detener el movimiento al atacar
        rb.velocity = Vector2.zero;

        animator.SetBool("isAttacking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isWalking", false);

        Debug.Log("Atacando al jugador!");
        // Implementar lógica de daño aquí si es necesario
    }

    void Flip()
    {
        // Cambia la dirección del sprite
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Manejar colisiones si es necesario, como con el suelo o el jugador
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Evitar atravesar el suelo
        }
    }
}
