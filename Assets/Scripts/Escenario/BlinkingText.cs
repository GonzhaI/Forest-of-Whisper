using System.Collections;
using UnityEngine;
using TMPro;

public class BlinkingText : MonoBehaviour
{
    public float blinkInterval = 1f; // Intervalo de parpadeo en segundos
    private TMP_Text textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        StopCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            textComponent.enabled = !textComponent.enabled; // Alternar la visibilidad
            yield return new WaitForSeconds(blinkInterval); // Esperar el intervalo
        }
    }
}
