using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGanador : MonoBehaviour
{

    public void ActivarMenu(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void NuevaPartidaPlus()
    {
        GameDataManager.Instance.health = 5; // Reiniciar vida
        GameDataManager.Instance.currentScene = "Tutorial"; // Reiniciar escena al tutorial
        StartCoroutine(GameDataManager.Instance.SaveGameData());
        SceneManager.LoadScene("Tutorial");
    }

}
