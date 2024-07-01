using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead { get; private set; }
    [SerializeField] private int maxHealth = 3; // Valor inicial de vidas
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private AudioClip damageAudioClip; // Agrega el clip de audio
    [SerializeField] private AudioSource damageAudioSource; // Agrega el AudioSource
    [SerializeField] private GameObject sword; // Agrega la referencia a la espada

    private int currentHealth;
    private bool canTakeDamage = true;
    private bool canMove = true;
    private Knockback knockback;
    private Flash flash;

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();

        // Cargar las vidas guardadas o inicializar con el valor máximo
        currentHealth = PlayerPrefs.GetInt("PlayerHealth", maxHealth);
    }

    private void Start()
    {
        isDead = false;
        UpdateHealthText();
    }

    private void OnDisable()
    {
        // Guardar las vidas al desactivar el script, solo si no está muerto
        if (!isDead)
        {
            PlayerPrefs.SetInt("PlayerHealth", currentHealth);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy && canMove)
        {
            TakeDamage(1, other.transform);
        }
    }

    private void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;

        // Reproducir el sonido de daño
        if (damageAudioSource != null && damageAudioClip != null)
        {
            damageAudioSource.clip = damageAudioClip;
            damageAudioSource.Play();
        }

        UpdateHealthText();
        StartCoroutine(DamageRecoveryRoutine());
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            canMove = false;
            currentHealth = 0;
            UpdateHealthText();

            // Desactivar la espada
            if (sword != null)
            {
                sword.SetActive(false);
            }

            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathRoutine());
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // Reinicia las vidas al máximo
        UpdateHealthText(); // Actualiza el texto de vidas
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthText()
    {
        if (TextManager.instance != null)
        {
            TextManager.instance.UpdateHealthText(currentHealth);
        }
    }

    private IEnumerator DeathRoutine()
    {
        // Llama a DisableMovement en PlayerController
        PlayerController.Instance.DisableMovement();

        yield return new WaitForSeconds(4f);

        // Habilitar el mouse después de 4 segundos
        PlayerController.Instance.EnableMouse();

        // Cargar la escena de juego
        SceneManager.LoadScene("Menu");
    }
}
