using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Variables para guardar el estado del jugador
    public int playerHealth;

    private void Awake()
    {
        // Implementación del patrón singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Para evitar duplicaciones innecesarias
        }

        // Inicializar las variables
        playerHealth = 3; // O el valor inicial que desees
    }

    // Métodos públicos para obtener y establecer playerHealth
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealth = health;
    }
}
