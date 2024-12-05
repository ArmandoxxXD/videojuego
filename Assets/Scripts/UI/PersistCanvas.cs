using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistCanvas : MonoBehaviour
{
    private static PersistCanvas instance;

    // Lista de escenas donde mostrar el Canvas
    public string[] scenesToShowCanvas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Activar o desactivar el Canvas según la escena
        bool showCanvas = false;
        foreach (string sceneName in scenesToShowCanvas)
        {
            if (scene.name == sceneName)
            {
                showCanvas = true;
                break;
            }
        }

        gameObject.SetActive(showCanvas);

        // Notificar que el Canvas está listo
        if (showCanvas)
        {
            CanvasReady();
        }
    }

    void CanvasReady()
    {
        Debug.Log("Canvas listo para el TutorialManager.");
        TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
        if (tutorialManager != null)
        {
            tutorialManager.InitializeCanvasReferences(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
