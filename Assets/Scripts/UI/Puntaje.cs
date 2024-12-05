using UnityEngine;
using TMPro;

public class Puntaje : MonoBehaviour
{
    private int puntos;
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        puntos = 0; // Inicializamos con 0 para evitar residuos
        textMesh.text = puntos.ToString();
    }

    void Update()
    {
        textMesh.text = puntos.ToString();
        GameDataManager.Instance.score = puntos;
    }

    public void SumarDiamantes(int puntosEntrada)
    {
        puntos += puntosEntrada;
    }

    public void InicializarPuntaje(int puntajeInicial)
    {
        puntos = puntajeInicial;
        textMesh.text = puntos.ToString();
    }
}
