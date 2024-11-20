using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CambiarNivelTrigger : MonoBehaviour
{
    public string nextSceneName; // Nombre de la escena a cargar
    public TutorialManager tutorialManager; // Referencia al TutorialManager
    private GameObject instructionTextObject; // Objeto del texto
    private TextMeshPro instructionText; // Componente TextMeshPro para el texto

    private bool isPlayerNear = false;

    void Start()
    {
        // Crear un objeto de texto 2D en el mundo
        instructionTextObject = new GameObject("InstructionText");
        instructionTextObject.transform.SetParent(transform); // Hacer que el texto sea hijo del objeto bandera
        instructionTextObject.transform.localPosition = new Vector3(0, 3.0f, 0); // Posicionar el texto sobre el objeto

        // Agregar un componente TextMeshPro al objeto
        instructionText = instructionTextObject.AddComponent<TextMeshPro>();
        instructionText.text = "Presiona 'E'\npara avanzar de nivel"; // Añadir un salto de línea
        instructionText.fontSize = 3; // Ajusta el tamaño del texto
        instructionText.alignment = TextAlignmentOptions.Center; // Centrar el texto
        instructionText.color = Color.white; // Color del texto

        // Desactivar el texto inicialmente
        instructionTextObject.SetActive(false);
    }

    void Update()
    {
        // Solo permitir interacción si el tutorial está completado
        if (isPlayerNear && tutorialManager != null && tutorialManager.tutorialCompleted)
        {
            instructionTextObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(nextSceneName); // Cargar la siguiente escena
            }
        }
        else
        {
            instructionTextObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            instructionTextObject.SetActive(false);
        }
    }
}
