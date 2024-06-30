using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public static TextManager instance;
    public TMP_Text scoreText;
    public TMP_Text scoreLabelText;
    public TMP_Text healthText;
    public TMP_Text healthLabelText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        FindTextObjects();
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
        FindTextObjects(); // Asegúrate de encontrar los objetos de texto al cargar la escena
        SetHealthLabelText();
        SetScoreLabelText();

        // Actualizar el texto de las vidas al cargar la escena
        UpdateHealthText(GameManager.instance.playerHealth);
    }

    private void FindTextObjects()
    {
        scoreText = GameObject.Find("Puntaje")?.GetComponent<TMP_Text>();
        scoreLabelText = GameObject.Find("Texto_puntaje")?.GetComponent<TMP_Text>();
        healthText = GameObject.Find("Vidas")?.GetComponent<TMP_Text>();
        healthLabelText = GameObject.Find("Texto_vidas")?.GetComponent<TMP_Text>();
    }

    public void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void UpdateHealthText(int health)
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }

    public void SetHealthText(string text)
    {
        if (healthText != null)
        {
            healthText.text = text;
        }
    }

    public void SetHealthLabelText()
    {
        if (healthLabelText != null)
        {
            healthLabelText.text = "Vidas:";
        }
    }

    public void SetScoreLabelText()
    {
        if (scoreLabelText != null)
        {
            scoreLabelText.text = "Puntaje:";
        }
    }
}
