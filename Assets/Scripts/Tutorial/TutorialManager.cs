using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.Playables; // Para manejar la animación
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    // Referencias dinámicas al Canvas y sus elementos
    private GameObject tutorialPanel;
    private TextMeshProUGUI tutorialText;
    private GameObject adKeysPanel;
    private GameObject mouseKeysPanel;
    private Image shiftKeyImage;
    private Image spaceKeyImage;

    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;
    public PlayableDirector entradaDirector;

    private int step = 0;
    public bool tutorialCompleted = false;

    private bool canvasInitialized = false;

    // Diccionarios para rastrear el estado de teclas y clics
    private Dictionary<KeyCode, bool> moveKeysPressed = new Dictionary<KeyCode, bool>
    {
        { KeyCode.A, false },
        { KeyCode.D, false },
        { KeyCode.LeftArrow, false },
        { KeyCode.RightArrow, false }
    };

    private Dictionary<int, bool> mouseClicksPressed = new Dictionary<int, bool>
    {
        { 0, false },  // Clic izquierdo
        { 1, false }   // Clic derecho
    };

    private void Start()
    {
        // Asegurarse de que el Canvas esté inicializado
        GameObject canvasInstance = FindObjectOfType<PersistCanvas>()?.gameObject;

        if (canvasInstance != null)
        {
            InitializeCanvasReferences(canvasInstance);
        }
        else
        {
            Debug.LogError("Canvas no encontrado. Asegúrate de que el Canvas esté cargado.");
        }

        if (IsInTutorialScene())
        {
            ResetTutorialState();

            // Iniciar el tutorial tras la animación de entrada
            if (entradaDirector != null)
            {
                entradaDirector.stopped += OnCinematicFinished; // Suscribir evento
            }
            else
            {
                StartTutorial(); // Iniciar directamente si no hay animación
            }
        }
        else
        {
            DisableTutorialPanel();
        }
    }

    public void InitializeCanvasReferences(GameObject canvas)
    {
        tutorialPanel = canvas.transform.Find("tutorialPanel")?.gameObject;
        tutorialText = canvas.transform.Find("tutorialPanel/tutorialText")?.GetComponent<TextMeshProUGUI>();
        adKeysPanel = canvas.transform.Find("tutorialPanel/adKeysPanel")?.gameObject;
        mouseKeysPanel = canvas.transform.Find("tutorialPanel/mouseKeysPanel")?.gameObject;
        shiftKeyImage = canvas.transform.Find("tutorialPanel/shiftKeyImage")?.GetComponent<Image>();
        spaceKeyImage = canvas.transform.Find("tutorialPanel/spaceKeyImage")?.GetComponent<Image>();

        if (tutorialPanel == null || tutorialText == null)
        {
            Debug.LogError("No se pudo inicializar el TutorialManager porque faltan referencias en el Canvas.");
            return;
        }

        canvasInitialized = true;
    }

    private void StartTutorial()
    {
        if (!canvasInitialized)
        {
            Debug.LogWarning("El Canvas aún no está inicializado para el TutorialManager.");
            return;
        }

        tutorialPanel.SetActive(true);
        ShowNextStep();
    }

    private void ShowNextStep()
    {
        if (!canvasInitialized) return;

        switch (step)
        {
            case 0:
                tutorialText.text = "Usa las teclas AD o las flechas para moverte";
                ResetUI();
                adKeysPanel?.SetActive(true);
                break;
            case 1:
                tutorialText.text = "Presiona SPACE para saltar";
                ResetUI();
                spaceKeyImage?.gameObject.SetActive(true);
                break;
            case 2:
                tutorialText.text = "Presiona SHIFT para hacer un dash.\nPodrás pasar por lugares estrechos.";
                ResetUI();
                shiftKeyImage?.gameObject.SetActive(true);
                break;
            case 3:
                tutorialText.text = "Presiona clic izquierdo para atacar y clic derecho para bloquear";
                ResetUI();
                mouseKeysPanel?.SetActive(true);
                break;
            case 4:
                StartCoroutine(PlayCinematicAndProceed());
                break;
            case 5:
                tutorialText.text = "¡Buen trabajo! Has completado el tutorial.\nVe hacia la bandera para continuar.";
                ResetUI();
                StartCoroutine(CompleteTutorial());
                break;
        }
    }

    private IEnumerator PlayCinematicAndProceed()
    {
        tutorialPanel.SetActive(false);
        yield return new WaitForSeconds(1f);

        if (cinematicCamera != null)
        {
            cinematicCamera.Priority = 10;
            playerCamera.Priority = 5;
            yield return new WaitForSeconds(3f);
            cinematicCamera.Priority = 5;
            playerCamera.Priority = 10;
        }

        tutorialPanel.SetActive(true);
        step++;
        ShowNextStep();
    }

    private IEnumerator CompleteTutorial()
    {
        yield return new WaitForSeconds(3f);
        tutorialPanel.SetActive(false);
        tutorialCompleted = true;
    }

    private void ResetUI()
    {
        adKeysPanel?.SetActive(false);
        mouseKeysPanel?.SetActive(false);
        shiftKeyImage?.gameObject.SetActive(false);
        spaceKeyImage?.gameObject.SetActive(false);
    }

    private void ResetTutorialState()
    {
        step = 0;
        tutorialCompleted = false;

        foreach (var key in new List<KeyCode>(moveKeysPressed.Keys))
        {
            moveKeysPressed[key] = false;
        }

        foreach (var click in new List<int>(mouseClicksPressed.Keys))
        {
            mouseClicksPressed[click] = false;
        }
    }

    private void Update()
    {
        if (!canvasInitialized || tutorialCompleted) return;

        switch (step)
        {
            case 0:
                CheckMovementKeys();
                break;
            case 1:
                CheckSpaceKey();
                break;
            case 2:
                CheckShiftKey();
                break;
            case 3:
                CheckMouseClicks();
                break;
        }
    }

    private void CheckMovementKeys()
    {
        foreach (var key in new List<KeyCode>(moveKeysPressed.Keys))
        {
            if (Input.GetKeyDown(key))
            {
                moveKeysPressed[key] = true;
            }
        }

        if ((moveKeysPressed[KeyCode.A] && moveKeysPressed[KeyCode.D]) ||
            (moveKeysPressed[KeyCode.LeftArrow] && moveKeysPressed[KeyCode.RightArrow]))
        {
            step++;
            ShowNextStep();
        }
    }

    private void CheckSpaceKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            step++;
            ShowNextStep();
        }
    }

    private void CheckShiftKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            step++;
            ShowNextStep();
        }
    }

    private void CheckMouseClicks()
    {
        foreach (var button in new List<int>(mouseClicksPressed.Keys))
        {
            if (Input.GetMouseButtonDown(button))
            {
                mouseClicksPressed[button] = true;
            }
        }

        if (mouseClicksPressed[0] && mouseClicksPressed[1])
        {
            step++;
            ShowNextStep();
        }
    }

    private bool IsInTutorialScene()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Tutorial";
    }

    private void DisableTutorialPanel()
    {
        tutorialPanel?.SetActive(false);
    }

    private void OnCinematicFinished(PlayableDirector director)
    {
        if (director == entradaDirector)
        {
            entradaDirector.stopped -= OnCinematicFinished; // Desuscribir evento
            StartTutorial();
        }
    }
}
