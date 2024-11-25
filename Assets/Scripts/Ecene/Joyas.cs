using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joyas : MonoBehaviour
{
    [SerializeField] private GameObject efecto;
    [SerializeField] private int cantidadPuntos;

    private Puntaje puntaje;

    private void Start()
    {
        // Buscar el componente Puntaje automáticamente en el Canvas persistente
        puntaje = FindObjectOfType<Puntaje>();

        if (puntaje == null)
        {
            Debug.LogError("No se encontró el componente Puntaje en la escena.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && puntaje != null)
        {
            puntaje.SumarDiamantes(cantidadPuntos);
            Instantiate(efecto, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
