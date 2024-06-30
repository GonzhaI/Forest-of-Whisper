using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public CambioDeNivel door;
    private int score = 0;
    private bool scoreInitialized = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantén el objeto al cambiar de escena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindDoorObject();
        UpdateScoreText();
    }

    private void Start()
    {
        if (!scoreInitialized)
        {
            scoreInitialized = true;
            // Inicializa el puntaje aquí si es necesario
            // score = 0;
        }

        FindDoorObject();
        UpdateScoreText();
    }

    private void FindDoorObject()
    {
        door = GameObject.FindObjectOfType<CambioDeNivel>();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        CheckDoorOpen();
    }

    public void ResetScore()
    {
        score = 0; // Reinicia el puntaje
        UpdateScoreText(); // Asegúrate de actualizar el texto en caso de que se necesite
    }

    private void UpdateScoreText()
    {
        if (TextManager.instance != null)
        {
            TextManager.instance.UpdateScoreText(score);
        }
    }

    private void CheckDoorOpen()
    {
        if (score >= 200 && door != null)
        {
            door.Open();
        }
    }
}
