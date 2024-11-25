using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class VidaJugador : MonoBehaviour
{
    public int vidaActual;
    public int vidaMaxima = 5;
    public UnityEvent<int> cambioVida;
    private Animator m_animator;
    public event EventHandler MuerteJugador;

    private GameObject menuGameOverPanel;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        vidaActual = GameDataManager.Instance.health;
        Debug.Log($"Vida inicializada desde la base de datos: {vidaActual}");
        cambioVida.Invoke(vidaActual);

        GameObject canvasInstance = FindObjectOfType<PersistCanvas>()?.gameObject;
        if (canvasInstance != null)
        {
            menuGameOverPanel = canvasInstance.transform.Find("MenuGameOver")?.gameObject;

            if (menuGameOverPanel == null)
            {
                Debug.LogError("No se encontró el panel MenuGameOver en el Canvas.");
            }
            else
            {
                menuGameOverPanel.SetActive(false); // Asegurarse de que esté desactivado al inicio
            }
        }
        else
        {
            Debug.LogError("Canvas persistente no encontrado. Asegúrate de que exista en la escena.");
        }
    }



    public void TomarDaño(int cantidadDaño)
    {
        int vidaTemporal = vidaActual - cantidadDaño;
        if(vidaTemporal < 0)
        {
            vidaActual = 0;
        } else
        {
            vidaActual = vidaTemporal;
        }

        cambioVida.Invoke(vidaActual);
        GameDataManager.Instance.health = vidaActual;

        if (vidaActual <= 0)
        {
            m_animator.SetTrigger("Death");
            MuerteJugador?.Invoke(this,EventArgs.Empty);
            StartCoroutine(MorirYDestruir());
        }

    }

    public void CurarVida(int cantidadCuracion)
    {
        int vidaTemporal = vidaActual + cantidadCuracion;
        if (vidaTemporal > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }
        else
        {
            vidaActual = vidaTemporal;
        }

        cambioVida.Invoke(vidaActual);
        GameDataManager.Instance.health = vidaActual;
    }

    private IEnumerator MorirYDestruir()
    {
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
        // Mostrar el panel de Game Over
        if (menuGameOverPanel != null)
        {
            menuGameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("No se puede mostrar el panel Game Over porque no está configurado.");
        }

        Destroy(gameObject);
    }

}
