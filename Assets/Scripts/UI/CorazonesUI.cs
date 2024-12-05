using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CorazonesUI : MonoBehaviour
{
    private List<Image> listaCorazones = new List<Image>();
    private GameObject corazonPrefab;
    private Sprite corazonLleno;
    private Sprite corazonVacio;
    private VidaJugador vidaJugador;

    private void Awake()
    {
        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento al destruir el objeto
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Cargar los recursos necesarios
        corazonPrefab = Resources.Load<GameObject>("BarraVidaCorazon");
        corazonLleno = Resources.Load<Sprite>("Heart");
        corazonVacio = Resources.Load<Sprite>("Heart2");

        if (corazonPrefab == null || corazonLleno == null || corazonVacio == null)
        {
            Debug.LogError("Faltan recursos en CorazonesUI. Verifica que los prefabs y sprites estén correctamente configurados.");
            return;
        }

        // Inicializar la UI para la escena actual
        InitializeCorazonesUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reasignar referencias y actualizar la UI al cargar un nuevo nivel
        InitializeCorazonesUI();
    }

    private void InitializeCorazonesUI()
    {
        // Buscar el objeto VidaJugador
        vidaJugador = FindObjectOfType<VidaJugador>();

        if (vidaJugador == null)
        {
            Debug.LogError("VidaJugador no encontrado en la escena. Verifica que esté presente.");
            return;
        }

        // Suscribirse al evento de cambio de vida
        vidaJugador.cambioVida.RemoveListener(ActualizarCorazones); // Evitar duplicar suscripciones
        vidaJugador.cambioVida.AddListener(ActualizarCorazones);

        // Crear corazones según la vida máxima
        CrearCorazones(vidaJugador.vidaMaxima);

        // Actualizar el estado inicial de los corazones
        ActualizarCorazones(vidaJugador.vidaActual);
    }

    private void CrearCorazones(int cantidadMaximaVida)
    {
        listaCorazones.Clear();

        // Eliminar corazones antiguos
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Crear nuevos corazones
        for (int i = 0; i < cantidadMaximaVida; i++)
        {
            GameObject corazon = Instantiate(corazonPrefab, transform);
            listaCorazones.Add(corazon.GetComponent<Image>());
        }
    }

    private void ActualizarCorazones(int vidaActual)
    {
        for (int i = 0; i < listaCorazones.Count; i++)
        {
            listaCorazones[i].sprite = i < vidaActual ? corazonLleno : corazonVacio;
        }
    }
}
