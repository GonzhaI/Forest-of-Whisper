using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText; // Cambiado a TMP_Text
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

    private void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        CheckDoorOpen(); // Verificar si se debe abrir la puerta
    }

    private void UpdateScoreText()
    {
        scoreText.text = "" + score.ToString();
    }

    private void CheckDoorOpen()
    {
        if (score >= 2000 && door != null)
        {
            door.Open(); // Llamar al método Open() en el script CambioDeNivel
        }
    }
}
