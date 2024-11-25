using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class MenuInicial : MonoBehaviour
{
    [SerializeField] private GameObject partidasPanel;
    [SerializeField] private Transform partidasContainer;
    [SerializeField] private GameObject partidaButtonPrefab;
    [SerializeField] private TextMeshProUGUI mensajeSinPartidas;

    private const string apiUrl = "http://localhost:3000";

    public void Jugar()
    {
        StartCoroutine(CrearNuevaPartida());
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void VerPartidas()
    {
        StartCoroutine(ObtenerPartidas());
    }

    private IEnumerator CrearNuevaPartida()
    {
        string playerId = System.Guid.NewGuid().ToString();

        GameDataManager.Instance.InitializeNewGame(playerId, "Tutorial");

        UnityWebRequest request = new UnityWebRequest($"{apiUrl}/game_data", "POST");
        string jsonBody = JsonUtility.ToJson(GameDataManager.Instance.GetGameData());

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Nueva partida creada correctamente.");
            PlayerPrefs.SetString("PlayerId", playerId);
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            Debug.LogError($"Error al crear nueva partida: {request.error}");
        }
    }

    private IEnumerator ObtenerPartidas()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{apiUrl}/game_data");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            foreach (Transform child in partidasContainer)
            {
                Destroy(child.gameObject);
            }

            var partidas = JsonUtility.FromJson<PartidaList>($"{{\"partidas\":{request.downloadHandler.text}}}");

            if (partidas.partidas.Length == 0)
            {
                mensajeSinPartidas.gameObject.SetActive(true);
            }
            else
            {
                mensajeSinPartidas.gameObject.SetActive(false);
                foreach (var partida in partidas.partidas)
                {
                    GameObject nuevaPartida = Instantiate(partidaButtonPrefab, partidasContainer);
                    TextMeshProUGUI buttonText = nuevaPartida.GetComponentInChildren<TextMeshProUGUI>();
                    buttonText.text = $"{partida.scene_name}, Puntos: {partida.score}";

                    nuevaPartida.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CargarPartida(partida));
                }
            }

            partidasPanel.SetActive(true);
        }
        else
        {
            Debug.LogError($"Error al obtener partidas: {request.error}");
        }
    }

    private void CargarPartida(Partida partida)
    {
        if (string.IsNullOrEmpty(partida.player_id))
        {
            Debug.LogError("Player ID de la partida seleccionada es nulo o vacío.");
            return;
        }

        Debug.Log($"Cargando partida con ID: {partida.player_id}");

        PlayerPrefs.SetString("PlayerId", partida.player_id); // Guarda el ID de la partida seleccionada
        GameDataManager.Instance.InitializeFromPartida(partida); // Inicializa el GameDataManager

        SceneManager.sceneLoaded += OnSceneLoaded; // Suscribirse al evento de carga de escena
        SceneManager.LoadScene(GameDataManager.Instance.currentScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameDataManager.Instance.SincronizarDatos(); // Sincronizar datos después de cargar la escena
        SceneManager.sceneLoaded -= OnSceneLoaded; // Desuscribirse del evento
    }

}

[System.Serializable]
public class Partida
{
    public string player_id;
    public string scene_name;
    public int score;
    public int health;
}

[System.Serializable]
public class PartidaList
{
    public Partida[] partidas;
}
