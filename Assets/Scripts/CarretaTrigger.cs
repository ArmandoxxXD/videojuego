using UnityEngine;
using UnityEngine.SceneManagement;

public class CarretaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el jugador ha tocado la carreta
        if (other.CompareTag("Player"))
        {
            // Cambia a la escena "Name_Escena"
            SceneManager.LoadScene("Nivell3");
        }
    }
}
