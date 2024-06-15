using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IniciarJuego : MonoBehaviour
{
    public string sceneToLoad; // Nombre de la escena a cargar, configurado desde el Inspector

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}