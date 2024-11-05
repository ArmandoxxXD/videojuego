using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float detectRange = 10.0f; // Rango de detección del jugador
    public float attackRange = 5.0f;  // Rango de ataque del rayo
    public int damage = 20;           // Daño del rayo
    public LineRenderer rayLine;      // LineRenderer para el rayo

    private Transform player;         // Referencia al jugador

    void Start()
    {
        // Encuentra al jugador usando la etiqueta "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Asegúrate de que el LineRenderer esté desactivado al inicio
        rayLine.enabled = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            // Si el jugador está dentro del rango de detección, mirar hacia él
            LookAtPlayer();

            if (distanceToPlayer <= attackRange)
            {
                AttackWithRay();
            }
            else
            {
                // Si está fuera del rango de ataque, desactiva el rayo
                rayLine.enabled = false;
            }
        }
        else
        {
            // Desactiva el rayo si el jugador está fuera del rango de detección
            rayLine.enabled = false;
        }
    }

    void LookAtPlayer()
    {
        // Hace que el fantasma mire hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void AttackWithRay()
    {
        // Activa el LineRenderer y ajusta el rayo
        rayLine.enabled = true;
        rayLine.SetPosition(0, transform.position);
        rayLine.SetPosition(1, player.position);

        // Verifica si el rayo golpea al jugador
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // Aquí puedes implementar el daño al jugador
                Debug.Log("El fantasma ataca con un rayo y causa " + damage + " de daño");
            }
        }
    }
}
