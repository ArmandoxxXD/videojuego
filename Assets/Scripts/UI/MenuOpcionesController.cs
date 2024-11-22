using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpcionesController : MonoBehaviour
{
    public GameObject menuInicial; // Referencia al menú principal

    void Start()
    {
        // Asegúrate de que el menú inicial esté desactivado al entrar al menú de opciones
        if (menuInicial != null)
        {
            menuInicial.SetActive(false);
        }
    }

    // Método para volver al menú principal
    public void VolverAlMenuPrincipal()
    {
        if (menuInicial != null)
        {
            // Desactivamos el menú de opciones y activamos el menú principal
            OptionsManager.Instance?.SaveOptions();
            gameObject.SetActive(false);
            menuInicial.SetActive(true);
        }
        else
        {
            Debug.LogError("menuInicial no está asignado en el Inspector.");
        }
    }


}
