using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject menuOpciones;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Activar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0;
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);

        Time.timeScale = 1;
    }

    public void ActivarMenu(string nombre)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nombre);
    }

    public void ActivarOpciones()
    {
        menuPausa.SetActive(false);
        menuOpciones.SetActive(true);
    }

    public void volverMenuPausa()
    {
        Time.timeScale = 0;
        menuPausa.SetActive(true);
       
    }
}
