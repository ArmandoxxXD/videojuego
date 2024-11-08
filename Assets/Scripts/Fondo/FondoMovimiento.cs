using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimeinto;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D player;
    private HeroKnight heroKnight;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        heroKnight = player.GetComponent<HeroKnight>();
    }

    private void Update()
    {
        // Solo mover el fondo si el jugador no está escalando (m_isWallSliding == false)
        if (!heroKnight.IsWallSliding)
        {
            offset = (player.velocity.x * 0.1f) * velocidadMovimeinto * Time.deltaTime;
            material.mainTextureOffset += offset;
        }
    }
}
