using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TMP_Text scoreText; // Cambiado a TMP_Text
    private int score = 0;

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
    }

    private void UpdateScoreText()
    {
        scoreText.text = "" + score.ToString();
    }
}
