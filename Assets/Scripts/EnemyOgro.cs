using UnityEngine;

public class Villain : MonoBehaviour
{
    // Variables para definir el comportamiento
    public float speed = 3.0f; // Velocidad del villano
    public float attackRange = 1.5f; // Rango de ataque
    public int damage = 10; // Da�o que hace al atacar

    private Transform player; // Referencia al jugador

    void Start()
    {
        // Encuentra al jugador usando la etiqueta "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Si el jugador est� dentro del rango de ataque, ataca
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Mueve al villano hacia el jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void Attack()
    {
        // Aqu� podr�as implementar el ataque
        Debug.Log("El villano est� atacando y causa " + damage + " de da�o");
    }
}
