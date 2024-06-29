using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText; // TextMeshPro para mostrar el puntaje actual
    public TMP_Text scoreLabelText; // TextMeshPro para el texto "Puntaje:"
    private int score = 0;
    public CambioDeNivel door; // Referencia al script CambioDeNivel

    private void Awake()
    {
        // Asegúrate de que solo hay una instancia de ScoreManager
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
        FindScoreText();
        UpdateScoreText();
    }

    private void Start()
    {
        FindScoreText();
        UpdateScoreText();
    }

    private void FindScoreText()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("Puntaje").GetComponent<TMP_Text>();
        }

        if (scoreLabelText == null)
        {
            scoreLabelText = GameObject.Find("Texto_puntaje").GetComponent<TMP_Text>();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        CheckDoorOpen(); // Verificar si se debe abrir la puerta
    }

    private void UpdateScoreText()
    {
        // Asegúrate de que los textos se actualicen correctamente
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (scoreLabelText != null)
        {
            // Asegúrate de que el texto "Puntaje:" no se vea afectado
            scoreLabelText.text = "Puntaje:";
        }
    }

    private void CheckDoorOpen()
    {
        if (score >= 500 && door != null)
        {
            door.Open(); // Llamar al método Open() en el script CambioDeNivel
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}
