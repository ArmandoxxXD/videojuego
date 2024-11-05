using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cainos.PixelArtMonster_Dungeon
{
    public class MonsterController : MonoBehaviour
    {
        //MOVEMENT PARAMETERS
        public float walkSpeedMax = 2.5f;                           // velocidad máxima de caminata del enemigo
        public float walkAcc = 5.0f;                                // aceleración de caminata

        public float attackRange = 8.0f;                            // distancia máxima para atacar al jugador
        private Transform playerTransform;                          // referencia al transform del jugador

        private PixelMonster pm;                                    // el script PixelMonster adjunto al personaje
        private Rigidbody2D rb2d;                                   // Rigidbody2D del enemigo

        private Vector2 curVel;                                     // velocidad actual
        private bool isGrounded;                                    // si el enemigo está en el suelo
        private bool isDead;                                        // estado de muerte del enemigo

        // Propiedad IsDead
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
                if (pm != null)
                {
                    pm.IsDead = isDead;
                }
            }
        }

        private void Awake()
        {
            pm = GetComponent<PixelMonster>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // Encuentra al jugador por su tag "Player"
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            if (playerTransform != null && !isDead) // Verifica que el enemigo no esté muerto
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                Debug.Log("Distancia al jugador: " + distanceToPlayer); // Verifica la distancia al jugador en la consola

                if (distanceToPlayer > attackRange)
                {
                    MoveTowardsPlayer();
                }
                else
                {
                    rb2d.velocity = Vector2.zero; // Detiene el movimiento al estar en rango
                    Debug.Log("Jugador en rango de ataque"); // Mensaje cuando el jugador está dentro del rango
                    AttackIfInRange();
                }
            }
        }

        private void MoveTowardsPlayer()
        {
            // Calcula la dirección hacia el jugador
            float direction = playerTransform.position.x - transform.position.x;
            direction = Mathf.Sign(direction); // 1 si el jugador está a la derecha, -1 si está a la izquierda

            // Configura la velocidad del enemigo en la dirección del jugador
            curVel = rb2d.velocity;
            curVel.x = direction * walkSpeedMax;
            rb2d.velocity = curVel;

            // Actualiza la dirección en el script PixelMonster para que el enemigo se oriente correctamente
            pm.Facing = Mathf.RoundToInt(direction);
        }

        private void AttackIfInRange()
        {
            // Solo ejecuta el ataque si el jugador está dentro del rango de ataque
            if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                Debug.Log("Ejecutando ataque"); // Mensaje de ataque ejecutado
                pm.Attack(); // Ejecuta la animación o acción de ataque
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Dibuja un círculo para visualizar el rango de ataque
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
