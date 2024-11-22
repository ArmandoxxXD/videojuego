using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class VidaJugador : MonoBehaviour
{
    public int vidaActual;
    public int vidaMaxima;
    public UnityEvent<int> cambioVida;
    private Animator m_animator;
    public event EventHandler MuerteJugador;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        vidaActual = vidaMaxima;
        cambioVida.Invoke(vidaActual);
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
    }

    private IEnumerator MorirYDestruir()
    {
        yield return new WaitForSeconds(m_animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

}
