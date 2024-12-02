using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class CambiarNivelTrigger : MonoBehaviour
{
    public string nextSceneName; // Nombre de la escena a cargar
    public CinemachineVirtualCamera cinematicCamera; // C�mara Cinem�tica
    public CinemachineVirtualCamera playerCamera; // C�mara del jugador
    public TutorialManager tutorialManager; // Referencia al TutorialManager
    private GameObject instructionTextObject; // Objeto del texto
    private TextMeshPro instructionText; // Componente TextMeshPro para el texto
    private EntradasMovimiento entradasMovimiento;

    private bool isPlayerNear = false;

    private void Awake()
    {
        entradasMovimiento = new EntradasMovimiento();
    }

    private void OnEnable()
    {
        entradasMovimiento.Enable();
    }

    private void OnDisable()
    {
        entradasMovimiento.Disable();
    }

    void Start()
    {
        // Crear un objeto de texto 2D en el mundo
        instructionTextObject = new GameObject("InstructionText");
        instructionTextObject.transform.SetParent(transform); // Hacer que el texto sea hijo del objeto bandera
        instructionTextObject.transform.localPosition = new Vector3(0, 3.0f, 0); // Posicionar el texto sobre el objeto

        // Agregar un componente TextMeshPro al objeto
        instructionText = instructionTextObject.AddComponent<TextMeshPro>();
        instructionText.text = ""; // Texto vac�o al inicio
        instructionText.fontSize = 3; // Ajusta el tama�o del texto
        instructionText.alignment = TextAlignmentOptions.Center; // Centrar el texto
        instructionText.color = Color.white; // Color del texto

        // Desactivar el texto inicialmente
        instructionTextObject.SetActive(false);

        // Reproducir la cinem�tica al cargar el nivel
        if (cinematicCamera != null && playerCamera != null)
        {
            StartCoroutine(PlayLevelIntroCinematic());
        }
    }

    void Update()
    {
        if (isPlayerNear)
        {
            // Condiciones para cambiar de nivel basadas en la escena actual
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                if (tutorialManager != null && tutorialManager.tutorialCompleted)
                {
                    MostrarTexto("Activa escudo\npara avanzar de nivel");
                    if (entradasMovimiento.Movimiento.Shell.triggered)
                    {
                        CambiarNivel();
                    }
                }
                else
                {
                    MostrarTexto("Completa el tutorial\npara avanzar");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Nivel1")
            {
                if (GameDataManager.Instance.score >= 5)
                {
                    MostrarTexto("Activa escudo\npara avanzar al Nivel 2");
                    if (entradasMovimiento.Movimiento.Shell.triggered)
                    {
                        CambiarNivel();
                    }
                }
                else
                {
                    MostrarTexto("Necesitas un score de 5\npara avanzar");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Nivel2")
            {
                if (GameDataManager.Instance.score >= 10)
                {
                    MostrarTexto("Activa escudo\npara avanzar al Nivel 3");
                    if (entradasMovimiento.Movimiento.Shell.triggered)
                    {
                        CambiarNivel();
                    }
                }
                else
                {
                    MostrarTexto("Necesitas un score de 10\npara avanzar");
                }
            }
            else if (SceneManager.GetActiveScene().name == "Nivel3")
            {
                if (GameDataManager.Instance.score >= 15)
                {
                    MostrarTexto("Activa escudo\npara terminar el juego");
                    if (entradasMovimiento.Movimiento.Shell.triggered)
                    {
                        CambiarNivel();
                    }
                }
                else
                {
                    MostrarTexto("Necesitas un score de 15\npara avanzar");
                }
            }
        }
        else
        {
            OcultarTexto();
        }
    }

    private IEnumerator PlayLevelIntroCinematic()
    {

        // Priorizar la c�mara player
        cinematicCamera.Priority = 5;
        playerCamera.Priority = 10;

        // Esperar la duraci�n de la cinem�tica (1 segundos ajustable)
        yield return new WaitForSeconds(1f);

        // Priorizar la c�mara cinem�tica
        cinematicCamera.Priority = 10;
        playerCamera.Priority = 5;

        // Esperar la duraci�n de la cinem�tica (3 segundos ajustable)
        yield return new WaitForSeconds(3f);

        // Restaurar la prioridad de la c�mara del jugador
        cinematicCamera.Priority = 5;
        playerCamera.Priority = 10;
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
            OcultarTexto();
        }
    }

    private void CambiarNivel()
    {
        GameDataManager.Instance.currentScene = nextSceneName;
        // Guardar datos antes de cargar la siguiente escena
        StartCoroutine(GameDataManager.Instance.SaveGameData());
        SceneManager.LoadScene(nextSceneName); // Cargar la siguiente escena
    }

    private void MostrarTexto(string mensaje)
    {
        instructionText.text = mensaje;
        instructionTextObject.SetActive(true);
    }

    private void OcultarTexto()
    {
        instructionTextObject.SetActive(false);
    }
}
