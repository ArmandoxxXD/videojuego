using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float detectionattackRadius = 1.5f;
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int dañoAtaque = 1;
    //[SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private Transform controladorAtaque;

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

        float distanceToPlayer = Vector2.Distance((Vector2)transform.position, player.position);


        if (distanceToPlayer < detectionattackRadius && !isAttacking)
        {
            StartAttack();

        } else if (distanceToPlayer < detectionRadius && !isAttacking)
        {
            isChasing = true;
            ChasePlayer();

        } else
        {
            isChasing = false;
            StopChase();
        }
        UpdateAnimations();
    }

    void ChasePlayer()
    {

        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
        movement = direction * speed;

        if (!isAttacking && ((movement.x > 0 && !facingRight) || (movement.x < 0 && facingRight)))
        {
            Flip();
        }


        rb.velocity = new Vector2(movement.x, rb.velocity.y);
    }

    void StopChase()
    {
        rb.velocity = Vector2.zero;
    }

    void StartAttack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero;
    }


    public void EndAttack()
    {
        isAttacking = false;
    }

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



    void Flip()
    {
        if (!isAttacking)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    void UpdateAnimations()
    {
        float absVelocityX = Mathf.Abs(rb.velocity.x);

        animator.SetBool("isWalking", isChasing && absVelocityX > 0);
        animator.SetBool("isAttacking", isAttacking);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionattackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(controladorAtaque.position, attackRadius);
    }

}
