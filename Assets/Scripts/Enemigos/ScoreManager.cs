using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public CambioDeNivel door;
    private int score = 0;
    private bool scoreInitialized = false;
    private int pointsNeeded = 200; // Puntos necesarios para abrir la puerta

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
        UpdatePointsNeeded(scene.name);
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
        if (score >= pointsNeeded && door != null)
        {
            door.Open();
        }
    }

    private void UpdatePointsNeeded(string sceneName)
    {
        switch (sceneName)
        {
            case "Nivel1":
                pointsNeeded = 200;
                break;
            case "Nivel2":
                pointsNeeded = 400;
                break;
            default:
                pointsNeeded = 200; // Valor predeterminado, ajusta según sea necesario
                break;
        }
    }
}
