using UnityEngine;
using UnityEngine.SceneManagement;

public class CarretaTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        /* Comprueba si el jugador ha tocado la carreta
        if (other.CompareTag("Player"))
        {
            // Cambia a la escena "Name_Escena"
            SceneManager.LoadScene("Nivell3");
        }*/
        // Comprueba si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Obtén el nombre de la escena actual
            string escenaActual = SceneManager.GetActiveScene().name;

            // Determina la escena siguiente según la actual
            switch (escenaActual)
            {
                case "Nivel1":
                    SceneManager.LoadScene("Nivel1.1");
                    break;
                case "Nivel1.1":
                    SceneManager.LoadScene("Nivel2");
                    break;
                case "Nivel2":
                    SceneManager.LoadScene("Nivell3");
                    break;
                default:
                    Debug.Log("No hay más niveles configurados.");
                    break;
            }
        }
    }
}
