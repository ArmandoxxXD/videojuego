using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistCanvas : MonoBehaviour
{
    private static PersistCanvas instance;

    // Lista de escenas donde quieres mostrar el Canvas
    public string[] scenesToShowCanvas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica si el nombre de la escena está en la lista de escenas donde mostrar el Canvas
        bool showCanvas = false;
        foreach (string sceneName in scenesToShowCanvas)
        {
            if (scene.name == sceneName)
            {
                showCanvas = true;
                break;
            }
        }

        gameObject.SetActive(showCanvas); // Activa o desactiva el Canvas según la escena
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Evitar fugas de memoria
    }
}
