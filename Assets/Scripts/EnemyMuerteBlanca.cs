using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteBlanca : MonoBehaviour
{
    [SerializeField] private float vida;
    private Animator animator;
    private SpriteRenderer spr;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
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
        animator.SetBool("isDead", true);
        StartCoroutine(DestruirDespuesDeAnimacion());
    }

    private IEnumerator DestruirDespuesDeAnimacion()
    {
        float duracionDeMuerte = animator.GetCurrentAnimatorStateInfo(0).length;
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
        spr.color = Color.red;
        yield return new WaitForSeconds(0.1f); // Mantener el color rojo por un breve momento
        spr.color = Color.white; // Regresar al color original
    }


}
