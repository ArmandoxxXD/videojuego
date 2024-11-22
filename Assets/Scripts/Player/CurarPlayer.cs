using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurarPlayer : MonoBehaviour
{
    public int curacion;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out VidaJugador vidaJugador))
        {
            vidaJugador.CurarVida(curacion);
        }
        Destroy(gameObject);
    }
}
