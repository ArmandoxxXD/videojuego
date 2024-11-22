using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private int puntos;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        textMesh.text = puntos.ToString();
    }

    public void SumarDiamantes(int puntosEntrada)
    {
        puntos += puntosEntrada;
    }
}
