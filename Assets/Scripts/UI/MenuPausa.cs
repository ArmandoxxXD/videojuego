using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject menuPausa;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Activar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1;
    }

    public void IrMenu(string nombre)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nombre);
    }
}
