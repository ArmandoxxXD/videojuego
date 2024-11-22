using UnityEditor;
using UnityEngine;
using Cinemachine;

public class SceneScreenshot : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // Referencia a la cámara virtual
    public string fileName = "Level1Background.png"; // Nombre del archivo
    public string folderPath = "Assets/Screenshots"; // Carpeta donde guardar la imagen

    [ContextMenu("Capture Screenshot")] // Agregar opción en el menú contextual
    public void CaptureScreenshot()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("No se ha asignado ninguna Cinemachine Virtual Camera.");
            return;
        }

        // Obtener la cámara real que está siendo controlada por Cinemachine
        Camera targetCamera = Camera.main; // Suponiendo que la Main Camera es la usada por Cinemachine

        if (targetCamera == null)
        {
            Debug.LogError("No se encontró una cámara principal (Main Camera) controlada por Cinemachine.");
            return;
        }

        // Crear la carpeta si no existe
        if (!System.IO.Directory.Exists(folderPath))
        {
            System.IO.Directory.CreateDirectory(folderPath);
        }

        string fullPath = System.IO.Path.Combine(folderPath, fileName);

        // Crear un RenderTexture para la cámara
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24); // Ajusta la resolución según sea necesario
        targetCamera.targetTexture = renderTexture;
        Texture2D screenshot = new Texture2D(1920, 1080, TextureFormat.RGB24, false);

        // Renderizar la cámara al RenderTexture
        targetCamera.Render();
        RenderTexture.active = renderTexture;

        // Leer los píxeles del RenderTexture
        screenshot.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
        screenshot.Apply();

        // Guardar la imagen como archivo PNG
        System.IO.File.WriteAllBytes(fullPath, screenshot.EncodeToPNG());
        Debug.Log($"Captura guardada en: {fullPath}");

        // Restaurar el estado original de la cámara y limpiar
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);
    }
}
