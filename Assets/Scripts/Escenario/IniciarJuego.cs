using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IniciarJuego : MonoBehaviour
{
    public string sceneToLoad; // Nombre de la escena a cargar, configurado desde el Inspector

    public void LoadScene()
    {
        ScoreManager.instance.ResetScore(); // Reinicia el puntaje al iniciar la partida
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.ResetHealth(); // Reinicia las vidas al iniciar la partida
        }
        SceneManager.LoadScene(sceneToLoad); // Carga la escena especificada
    }
}
