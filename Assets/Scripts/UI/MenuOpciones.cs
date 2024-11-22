using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        // Cargar las configuraciones al inicio
        if (OptionsManager.Instance != null)
        {
            audioMixer.SetFloat("Volumen", OptionsManager.Instance.volume);
            Screen.fullScreen = OptionsManager.Instance.isFullScreen;
            QualitySettings.SetQualityLevel(OptionsManager.Instance.qualityIndex);
        }
    }

    public void PantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
        OptionsManager.Instance?.SetFullScreen(pantallaCompleta); // Sincronizar
    }

    public void CambiarVolumen(float volumen)
    {
        audioMixer.SetFloat("Volumen", volumen);
        OptionsManager.Instance?.SetVolume(volumen); // Sincronizar
    }

    public void CambiarCalidad(int index)
    {
        QualitySettings.SetQualityLevel(index);
        OptionsManager.Instance?.SetQuality(index); // Sincronizar
    }
}
