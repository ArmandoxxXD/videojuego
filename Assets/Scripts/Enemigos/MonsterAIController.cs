using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class MonsterAIController : MonoBehaviour
    {
        public Transform player;
        [SerializeField] private float detectionRadius = 5.0f;
        [SerializeField] private float detectionattackRadius = 1.5f;
        [SerializeField] private float attackRadius = 1.5f;
        [SerializeField] private float patrolDistance = 3.0f;
        [SerializeField] private int dañoAtaque = 1;
        [SerializeField] private Transform controladorAtaque;

        private Vector2 leftPatrolPoint;
        private Vector2 rightPatrolPoint;
        private bool isChasing = false;
        private bool isAttacking = false;
        private int direction = 1;
        private bool facingRight = true;

        private MonsterFlyingController flyingController;
        private PixelMonster pixelMonster;

        private void Awake()
        {
            flyingController = GetComponent<MonsterFlyingController>();
            pixelMonster = GetComponent<PixelMonster>();

            leftPatrolPoint = new Vector2(transform.position.x - patrolDistance, transform.position.y);
            rightPatrolPoint = new Vector2(transform.position.x + patrolDistance, transform.position.y);
        }

        private void Update()
        {
            if (pixelMonster.IsDead) return;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionattackRadius && !isAttacking)
            {
                isChasing = false;
                isAttacking = true;
                flyingController.inputAttack = true;
            }
            else if (distanceToPlayer <= detectionRadius && !isAttacking)
            {
                isChasing = true;
                isAttacking = false;
                flyingController.inputMove = (player.position - transform.position).normalized;
                FlipTowardsPlayer();
            }
            else
            {
                isChasing = false;
                isAttacking = false;
                flyingController.inputAttack = false;
                Patrol();
            }
        }

        private void Patrol()
        {
            Vector2 targetPatrolPoint = direction == 1 ? rightPatrolPoint : leftPatrolPoint;
            Vector2 directionToTarget = (targetPatrolPoint - (Vector2)transform.position).normalized;

            flyingController.inputMove = directionToTarget;

            if (Vector2.Distance(transform.position, targetPatrolPoint) < 0.2f)
            {
                direction *= -1;
                FlipDirection();
            }
        }

        private void FlipTowardsPlayer()
        {
            bool shouldFaceRight = player.position.x > transform.position.x;
            if (shouldFaceRight != facingRight)
            {
                FlipDirection();
            }
        }

        private void FlipDirection()
        {
            facingRight = !facingRight;
            direction = facingRight ? 1 : -1;

            // Actualiza la dirección del sprite/render utilizando la propiedad Facing de PixelMonster
            pixelMonster.Facing = direction;

            // Cambia la posición del controladorAtaque al otro lado del enemigo
            Vector3 attackPosition = controladorAtaque.localPosition;
            attackPosition.x = Mathf.Abs(attackPosition.x) * direction;
            controladorAtaque.localPosition = attackPosition;
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionattackRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(controladorAtaque.position, attackRadius);

            Vector2 tempLeftPatrolPoint = new Vector2(transform.position.x - patrolDistance, transform.position.y);
            Vector2 tempRightPatrolPoint = new Vector2(transform.position.x + patrolDistance, transform.position.y);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(new Vector3(tempLeftPatrolPoint.x, tempLeftPatrolPoint.y, transform.position.z), 0.1f);
            Gizmos.DrawSphere(new Vector3(tempRightPatrolPoint.x, tempRightPatrolPoint.y, transform.position.z), 0.1f);
        }
    }
}
