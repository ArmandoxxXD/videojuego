using UnityEngine;

public class ViewPanelMovil : MonoBehaviour
{
    [SerializeField]
    private GameObject mobileControlPanel; // El panel de controles para móvil

    private bool isMobileDevice = false;

    void Awake()
    {
        // Detectar si el dispositivo es móvil
        isMobileDevice = Application.isMobilePlatform;

        // Configurar el estado inicial del panel
        UpdatePanelState();
    }

    void UpdatePanelState()
    {
        if (isMobileDevice)
        {
            // Activar el panel en dispositivos móviles
            if (!mobileControlPanel.activeSelf)
            {
                mobileControlPanel.SetActive(true);
            }
        }
        else
        {
            // Desactivar el panel en otros dispositivos y asegurar que no se pueda activar
            if (mobileControlPanel.activeSelf)
            {
                mobileControlPanel.SetActive(false);
            }

            // Evitar que el panel sea activado en dispositivos no móviles
            PreventActivationOnNonMobile();
        }
    }

    void PreventActivationOnNonMobile()
    {
        if (mobileControlPanel != null)
        {
            // Deshabilitar el GameObject de forma que SetActive no tenga efecto
            Destroy(mobileControlPanel);
            Debug.LogWarning("El panel de controles móviles no puede activarse en dispositivos no móviles.");
        }
    }
}
