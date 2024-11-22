using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    private bool wasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!wasTriggered && other.CompareTag("Player"))
        {
            // Verifica si el TutorialManager aún existe
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.StartTutorial(); // Llama al tutorial
                wasTriggered = true; // Marca como activado
                gameObject.SetActive(false); // Desactiva el trigger
            }
            else
            {
                Debug.LogError("TutorialManager no encontrado en la escena.");
            }
        }
    }

    private void OnEnable()
    {
        // Restablece el estado del trigger al habilitar el GameObject
        wasTriggered = false;
    }
}
