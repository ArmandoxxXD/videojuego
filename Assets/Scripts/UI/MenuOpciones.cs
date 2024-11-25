using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Dropdown qualityDropdown;

    private const string FullscreenKey = "Fullscreen";
    private const string VolumeKey = "Volume";
    private const string QualityKey = "Quality";

    private void Start()
    {
        // Cargar configuraciones guardadas
        CargarConfiguraciones();

        // Configurar listeners de la UI
        if (fullscreenToggle != null)
            fullscreenToggle.onValueChanged.AddListener(PantallaCompleta);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(CambiarVolumen);

        if (qualityDropdown != null)
            qualityDropdown.onValueChanged.AddListener(CambiarCalidad);
    }

    public void PantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        PlayerPrefs.SetInt(FullscreenKey, pantallaCompleta ? 1 : 0);
    }

    public void CambiarVolumen(float volumen)
    {
        // Aplicar el volumen directamente
        audioMixer.SetFloat("Volumen", volumen);

        // Guardar el valor del slider
        PlayerPrefs.SetFloat(VolumeKey, volumen);
    }

    public void CambiarCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt(QualityKey, index);
    }

    private void CargarConfiguraciones()
    {
        // Cargar estado de pantalla completa
        bool isFullscreen = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;
        Screen.fullScreen = isFullscreen;
        if (fullscreenToggle != null)
            fullscreenToggle.isOn = isFullscreen;

        // Cargar volumen
        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 0f); // Valor por defecto: 0 dB
        audioMixer.SetFloat("Volumen", savedVolume);
        if (volumeSlider != null)
            volumeSlider.value = savedVolume;

        // Cargar calidad gráfica
        int qualityIndex = PlayerPrefs.GetInt(QualityKey, 2); // Calidad por defecto: 2 (medio)
        QualitySettings.SetQualityLevel(qualityIndex);
        if (qualityDropdown != null)
            qualityDropdown.value = qualityIndex;
    }
}
