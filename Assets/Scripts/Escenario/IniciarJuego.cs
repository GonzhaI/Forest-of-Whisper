using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IniciarJuego : MonoBehaviour
{
    public string sceneToLoad; // Nombre de la escena a cargar, configurado desde el Inspector

    public void LoadScene()
    {
        ScoreManager scoreManager = ScoreManager.instance;
        if (scoreManager != null)
        {
            scoreManager.ResetScore(); // Reinicia el puntaje al iniciar la partida
        }
        else
        {
            Debug.LogError("Error: ScoreManager.instance is not set.");
        }

        // Reinicia el valor de PlayerHealth a 5 al cargar una nueva partida
        PlayerPrefs.SetInt("PlayerHealth", 5);

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth(); // Reinicia las vidas al iniciar la partida
        }
        else
        {
            Debug.LogError("Error: PlayerHealth not found in scene.");
        }

        SceneManager.LoadScene(sceneToLoad); // Carga la escena especificada
    }
}
