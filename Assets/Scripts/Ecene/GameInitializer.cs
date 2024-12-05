using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject canvasPrefab; // Prefab del canvas persistente

    void Awake()
    {
        // Instanciar el Canvas si no existe
        if (FindObjectOfType<PersistCanvas>() == null && canvasPrefab != null)
        {
            GameObject canvasInstance = Instantiate(canvasPrefab);
            DontDestroyOnLoad(canvasInstance);
        }
    }
}
