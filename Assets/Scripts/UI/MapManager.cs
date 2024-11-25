using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public string levelName; // Nombre del nivel
        public Button button;    // Botón asociado
        public GameObject markerImage; // Imagen asociada como marcador
    }

    [SerializeField] private List<LevelButton> levelButtons; // Lista de niveles y botones

    private void Start()
    {
        // Actualizar los marcadores al inicio
        UpdatePlayerMarkers();

        // Registrar el evento para cuando se cargue una nueva escena
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Escuchar el evento de datos cargados
        GameDataManager.Instance.OnDataLoaded.AddListener(UpdatePlayerMarkers);
    }

    private void OnDestroy()
    {
        // Desregistrar eventos para evitar errores
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.OnDataLoaded.RemoveListener(UpdatePlayerMarkers);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Actualizar los marcadores después de cargar una nueva escena
        UpdatePlayerMarkers();
    }

    private void UpdatePlayerMarkers()
    {
        // Obtener el nombre del nivel actual desde el GameDataManager
        string currentScene = GameDataManager.Instance?.currentScene ?? SceneManager.GetActiveScene().name;

        foreach (var levelButton in levelButtons)
        {
            // Activar la imagen del marcador solo para el nivel actual
            if (levelButton.markerImage != null)
            {
                levelButton.markerImage.SetActive(levelButton.levelName == currentScene);
            }
        }
    }
}
