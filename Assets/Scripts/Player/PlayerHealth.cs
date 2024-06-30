using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead { get; private set; }
    [SerializeField] private int maxHealth = 3; // Valor inicial de vidas
    [SerializeField] private float knockBackTrhustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

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

        // Inicializar currentHealth con maxHealth al inicio
        currentHealth = maxHealth;
    }

    private void Start()
    {
        isDead = false;
        UpdateHealthText();
    }

    private void OnDisable()
    {
        // Guardar las vidas al desactivar el script
        if (GameManager.instance != null)
        {
            GameManager.instance.playerHealth = currentHealth;
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

        knockback.GetKnockedBack(hitTransform, knockBackTrhustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;

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

            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathRoutine());
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth; // Reinicia las vidas al máximo
        UpdateHealthText(); // Actualiza el texto de vidas

        // Actualiza las vidas en el GameManager si está presente
        if (GameManager.instance != null)
        {
            GameManager.instance.playerHealth = currentHealth;
        }
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
        yield return new WaitForSeconds(4f);

        // Cargar la escena de juego
        SceneManager.LoadScene("Menu");
    }
}