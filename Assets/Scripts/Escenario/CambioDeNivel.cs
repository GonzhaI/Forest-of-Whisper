using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeNivel : MonoBehaviour
{
    public Sprite openDoorSprite; // El sprite de la puerta abierta
    public string sceneToLoad; // El nombre de la escena a cargar

    private SpriteRenderer spriteRenderer;
    private bool isDoorOpen = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        spriteRenderer.sprite = openDoorSprite; // Cambia el sprite de la puerta
        isDoorOpen = true; // Marca la puerta como abierta
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDoorOpen && collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad); // Cambia de escena
        }
    }
}
