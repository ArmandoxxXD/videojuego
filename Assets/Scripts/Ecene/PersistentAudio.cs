using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    private static PersistentAudio instance;

    [System.Serializable]
    public class LevelAudio
    {
        public string sceneName; // Nombre de la escena
        public AudioClip audioClip; // Canción asociada a la escena
    }

    [SerializeField] private LevelAudio[] levelAudios; // Lista de canciones para cada nivel
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>(); // Obtener el AudioSource en este objeto
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Buscar la canción asociada al nivel actual
        foreach (var levelAudio in levelAudios)
        {
            if (levelAudio.sceneName == scene.name)
            {
                if (audioSource.clip != levelAudio.audioClip)
                {
                    audioSource.clip = levelAudio.audioClip;
                    audioSource.Play();
                }
                return;
            }
        }

        Debug.LogWarning($"No se encontró una canción asociada para la escena: {scene.name}");
    }
}
