using Cainos.PixelArtMonster_Dungeon;
using System.Collections;
using UnityEngine;

public class EnemigoCainos : MonoBehaviour
{
    [SerializeField] private float vida;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool isDead = false;
    private MonsterFlyingController flyingController;
    private PixelMonster pixelMonster;

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        flyingController = GetComponent<MonsterFlyingController>();
        pixelMonster = GetComponent<PixelMonster>();
    }

    public void TomarDaño(int daño)
    {
        if (isDead) return;

        vida -= daño;
        RecibirGolpe();

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        isDead = true;
        flyingController.IsDead = true;  // Desactivar el movimiento y cambiar el estado a "muerto"
        pixelMonster.IsDead = true;      // Activar la animación de muerte en PixelMonster
        StartCoroutine(DestruirDespuesDeAnimacion());
    }

    private IEnumerator DestruirDespuesDeAnimacion()
    {
        float duracionDeMuerte = pixelMonster.animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(duracionDeMuerte);

        Destroy(gameObject);
    }

    public void RecibirGolpe()
    {
        if (!isDead)
        {
            StartCoroutine(CambiarColorTemporalmente());
        }
    }

    private IEnumerator CambiarColorTemporalmente()
    {
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.material.color = Color.red; // Cambiar a rojo al recibir daño
            yield return new WaitForSeconds(0.1f); // Mantener el color rojo por un breve momento
            skinnedMeshRenderer.material.color = Color.white; // Regresar al color original
        }
    }
}
