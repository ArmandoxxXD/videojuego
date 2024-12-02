using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    // Referencias dinámicas al Canvas y sus elementos
    private GameObject tutorialPanel;
    private TextMeshProUGUI tutorialText;
    private GameObject adKeysPanel;
    private GameObject mouseKeysPanel;
    private Image shiftKeyImage;
    private Image spaceKeyImage;
    private Image junpKeyImageMovil;
    private Image dashKeyImageMovil;

    public CinemachineVirtualCamera cinematicCamera;
    public CinemachineVirtualCamera playerCamera;
    public PlayableDirector entradaDirector;

    private int step = 0;
    public bool tutorialCompleted = false;

    private bool canvasInitialized = false;

    // Sistema de entradas
    private EntradasMovimiento entradasMovimiento;

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
        junpKeyImageMovil = canvas.transform.Find("tutorialPanel/JunpKeyImageMovil")?.GetComponent<Image>();
        dashKeyImageMovil = canvas.transform.Find("tutorialPanel/DashKeyImageMovil")?.GetComponent<Image>();

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
                tutorialText.text = IsPC()
                    ? "Usa las teclas AD o las flechas para moverte"
                    : "Usa los botones de movimiento en pantalla";
                ResetUI();
                adKeysPanel?.SetActive(IsPC());
                break;
            case 1:
                tutorialText.text = IsPC()
                    ? "Presiona SPACE para saltar"
                    : "Presiona el botón de salto en la pantalla";
                ResetUI();
                if (IsPC())
                    spaceKeyImage?.gameObject.SetActive(true);
                else
                    junpKeyImageMovil?.gameObject.SetActive(true);
                break;
            case 2:
                tutorialText.text = IsPC()
                    ? "Presiona SHIFT para hacer un dash.\nPodrás pasar por lugares estrechos."
                    : "Presiona el botón de dash en la pantalla";
                ResetUI();
                if (IsPC())
                    shiftKeyImage?.gameObject.SetActive(true);
                else
                    dashKeyImageMovil?.gameObject.SetActive(true);
                break;
            case 3:
                tutorialText.text = IsPC()
                    ? "Presiona clic izquierdo para atacar y clic derecho para bloquear"
                    : "Usa los botones de ataque y bloqueo en la pantalla";
                ResetUI();
                mouseKeysPanel?.SetActive(IsPC());
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
        junpKeyImageMovil?.gameObject.SetActive(false);
        dashKeyImageMovil?.gameObject.SetActive(false);
    }

    private void ResetTutorialState()
    {
        step = 0;
        tutorialCompleted = false;
    }

    private void Update()
    {
        if (!canvasInitialized || tutorialCompleted) return;

        switch (step)
        {
            case 0:
                CheckMovement();
                break;
            case 1:
                CheckJump();
                break;
            case 2:
                CheckDash();
                break;
            case 3:
                CheckActions();
                break;
        }
    }

    private void CheckMovement()
    {
        if (entradasMovimiento.Movimiento.Horizontal.ReadValue<float>() != 0)
        {
            step++;
            ShowNextStep();
        }
    }

    private void CheckJump()
    {
        if (entradasMovimiento.Movimiento.Salto.triggered)
        {
            step++;
            ShowNextStep();
        }
    }

    private void CheckDash()
    {
        if (entradasMovimiento.Movimiento.Dash.triggered)
        {
            step++;
            ShowNextStep();
        }
    }

    private void CheckActions()
    {
        if (entradasMovimiento.Movimiento.Atack.triggered || entradasMovimiento.Movimiento.Shell.triggered)
        {
            step++;
            ShowNextStep();
        }
    }

    private bool IsPC()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer ||
               Application.platform == RuntimePlatform.OSXPlayer ||
               Application.platform == RuntimePlatform.LinuxPlayer ||
               Application.platform == RuntimePlatform.WindowsEditor ||
               Application.platform == RuntimePlatform.OSXEditor;
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
