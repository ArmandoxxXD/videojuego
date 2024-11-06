using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) // Cambia a Collider2D y OnTriggerEnter2D
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<TutorialManager>().StartTutorial();
            gameObject.SetActive(false); // Desactiva el trigger después de iniciar el tutorial
        }
    }
}
