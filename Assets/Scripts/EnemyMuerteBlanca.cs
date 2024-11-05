using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float detectRange = 10.0f; // Rango de detecci�n del jugador
    public float attackRange = 5.0f;  // Rango de ataque del rayo
    public int damage = 20;           // Da�o del rayo
    public LineRenderer rayLine;      // LineRenderer para el rayo

    private Transform player;         // Referencia al jugador

    void Start()
    {
        // Encuentra al jugador usando la etiqueta "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Aseg�rate de que el LineRenderer est� desactivado al inicio
        rayLine.enabled = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            // Si el jugador est� dentro del rango de detecci�n, mirar hacia �l
            LookAtPlayer();

            if (distanceToPlayer <= attackRange)
            {
                AttackWithRay();
            }
            else
            {
                // Si est� fuera del rango de ataque, desactiva el rayo
                rayLine.enabled = false;
            }
        }
        else
        {
            // Desactiva el rayo si el jugador est� fuera del rango de detecci�n
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
                // Aqu� puedes implementar el da�o al jugador
                Debug.Log("El fantasma ataca con un rayo y causa " + damage + " de da�o");
            }
        }
    }
}
