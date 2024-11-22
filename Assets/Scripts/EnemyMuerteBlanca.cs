using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecterController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int maxAttacks = 3; // Número máximo de ataques antes de desaparecer
    [SerializeField] private int dañoAtaque = 1;
    [SerializeField] private Transform controladorAtaque;

    private Rigidbody2D rb;
    private Animator animator;
    private int attackCount = 0; // Contador de ataques
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < attackRadius && !isAttacking)
        {
            StartAttack();
        }
        else if (distanceToPlayer < detectionRadius && !isAttacking)
        {
            ChasePlayer();
        }
        else
        {
            StopChase();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void StopChase()
    {
        rb.velocity = Vector2.zero;
    }

    void StartAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");

        attackCount++;

        if (attackCount >= maxAttacks)
        {
            Die();
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    // Método para realizar el ataque al jugador
    public void AttackPlayer()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, attackRadius);

        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Player"))
            {
                HeroKnight hero = colision.GetComponent<HeroKnight>();
                if (hero != null)
                {
                    hero.TomarGolpe(dañoAtaque, transform.position);
                }
                break;
            }
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        rb.velocity = Vector2.zero;
        Destroy(gameObject, 1.5f); // Destruye el espectro después de 1.5 segundos para permitir la animación de desaparición
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(controladorAtaque.position, attackRadius);
    }
}
