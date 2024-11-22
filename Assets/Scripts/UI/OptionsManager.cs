using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance;

    [Header("Configuraciones")]
    public float volume = 0f; // Volumen actual
    public bool isFullScreen = true; // Modo pantalla completa
    public int qualityIndex = 0; // Índice de calidad

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantener entre escenas
            LoadOptions();
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
    }

    public void SetFullScreen(bool fullScreen)
    {
        isFullScreen = fullScreen;
    }

    public void SetQuality(int quality)
    {
        qualityIndex = quality;
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("Volumen", volume);
        PlayerPrefs.SetInt("PantallaCompleta", isFullScreen ? 1 : 0);
        PlayerPrefs.SetInt("Calidad", qualityIndex);
        PlayerPrefs.Save();
    }

    public void LoadOptions()
    {
        if (PlayerPrefs.HasKey("Volumen"))
        {
            volume = PlayerPrefs.GetFloat("Volumen");
            isFullScreen = PlayerPrefs.GetInt("PantallaCompleta") == 1;
            qualityIndex = PlayerPrefs.GetInt("Calidad");
        }
    }
}
