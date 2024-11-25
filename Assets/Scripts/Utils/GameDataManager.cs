using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    public UnityEvent OnDataLoaded = new UnityEvent();

    public string playerId;
    public int score;
    public int health;
    public string currentScene;

    //private const string apiUrl = "http://localhost:3000";
    private const string apiUrl = "https://back-end-videojuego.onrender.com";

    private void Start()
    {
        playerId = PlayerPrefs.GetString("PlayerId", string.Empty);
        if (!string.IsNullOrEmpty(playerId))
        {
            Debug.Log($"Cargando datos para PlayerId: {playerId}");
            StartCoroutine(LoadGameData(playerId));
        }
        else
        {
            Debug.LogError("Player ID no encontrado en PlayerPrefs.");
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoadGameData(string playerId)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/game_data/{playerId}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GameData data = JsonUtility.FromJson<GameData>(request.downloadHandler.text);
            this.playerId = data.player_id;
            score = data.score;
            health = data.health;
            currentScene = data.scene_name;

            Debug.Log("Datos del juego cargados correctamente.");
            OnDataLoaded.AddListener(SincronizarDatos);
            OnDataLoaded.Invoke();
        }
        else
        {
            Debug.LogError($"Error al cargar datos: {request.error}");
        }
    }

    public IEnumerator SaveGameData()
    {
        if (string.IsNullOrEmpty(playerId))
        {
            Debug.LogError("Player ID es nulo o vacío.");
            yield break;
        }

        GameData data = new GameData
        {
            player_id = playerId,
            score = score,
            health = health,
            scene_name = currentScene
        };

        string jsonBody = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest($"{apiUrl}/game_data/{playerId}", "PUT");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Datos del juego actualizados correctamente.");
        }
        else
        {
            Debug.LogError($"Error al actualizar los datos del juego: {request.error}");
        }
    }

    public void InitializeNewGame(string playerId, string initialScene)
    {
        this.playerId = playerId;
        score = 0; // Reiniciar puntaje
        health = 5; // Vida inicial
        currentScene = initialScene; // Escena inicial

        Debug.Log("Datos reiniciados para nueva partida.");
    }

    public void SincronizarDatos()
    {
        // Sincronizar vida
        VidaJugador vidaJugador = FindObjectOfType<VidaJugador>();
        if (vidaJugador != null)
        {
            vidaJugador.vidaActual = health;
            vidaJugador.cambioVida.Invoke(health);
        }

        // Sincronizar puntaje
        Puntaje puntaje = FindObjectOfType<Puntaje>();
        if (puntaje != null)
        {
            puntaje.InicializarPuntaje(score);
        }
    }

    public GameData GetGameData()
    {
        return new GameData
        {
            player_id = playerId,
            score = score,
            health = health,
            scene_name = currentScene
        };
    }

    public void InitializeFromPartida(Partida partida)
    {
        playerId = partida.player_id;
        score = partida.score;
        health = partida.health;
        currentScene = partida.scene_name;

        Debug.Log($"Partida inicializada: " +
                  $"PlayerId: {playerId}, Score: {score}, Health: {health}, Scene: {currentScene}");
    }
}

[System.Serializable]
public class GameData
{
    public string player_id;
    public int score;
    public int health;
    public string scene_name;
}
