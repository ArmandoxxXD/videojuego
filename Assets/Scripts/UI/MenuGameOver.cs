using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;

public class MenuGameOver : MonoBehaviour
{
    [SerializeField] private GameObject menuGameOver;
    private VidaJugador vidaJugador;

    //private const string apiUrl = "http://localhost:3000";
    private const string apiUrl = "https://back-end-videojuego.onrender.com";

    private void Start()
    {
        vidaJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<VidaJugador>();
        vidaJugador.MuerteJugador += ActivarMenu;
    }

    private void ActivarMenu(object sender, EventArgs e)
    {
        menuGameOver.SetActive(true);
    }

    public void Reiniciar()
    {
        StartCoroutine(ReiniciarPartida());
        Time.timeScale = 1;
    }

    public void MenuInicial(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    private IEnumerator ReiniciarPartida()
    {
        // Obtener el Player ID desde PlayerPrefs
        string playerId = PlayerPrefs.GetString("PlayerId", string.Empty);
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("No se encontró un Player ID válido en PlayerPrefs.");
            yield break;
        }

        Debug.Log($"Reiniciando partida para Player ID: {playerId}");

        // Hacer una petición para obtener los datos de la partida desde la base de datos
        UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/game_data/{playerId}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Obtener los datos de la partida
            GameData partida = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
            GameDataManager.Instance.InitializeFromPartida(new Partida
            {
                player_id = partida.player_id,
                scene_name = partida.scene_name,
                health = partida.health,
                score = partida.score
            });

            // Cargar la escena de la partida
            SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
            SceneManager.LoadScene(partida.scene_name);
        }
        else
        {
            Debug.LogError($"Error al cargar la partida: {request.error}");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Sincronizar los datos después de cargar la escena
        GameDataManager.Instance.SincronizarDatos();
        SceneManager.sceneLoaded -= OnSceneLoaded; // Desuscribirse del evento
    }
}
