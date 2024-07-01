using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public bool isDead { get; private set; }
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackTrhustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private TMP_Text healthText; // Campo para el texto de la vida del jugador
    [SerializeField] private AudioClip damageAudioClip;
    [SerializeField] private AudioSource damageAudioSource;

    public string sceneToLoad;
    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    readonly int DEATH_HASH = Animator.StringToHash("Death");

    private void Awake() {
        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
    }

    private void Start() {
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthText(); // Actualizar el texto al inicio
    }

    private void OnCollisionStay2D(Collision2D other) {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            TakeDamage(1, other.transform);
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform) {
        if (!canTakeDamage) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackTrhustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;

        if (damageAudioSource != null && damageAudioClip != null) {
            damageAudioSource.clip = damageAudioClip;
            damageAudioSource.Play();
        }

        UpdateHealthText(); // Actualizar el texto después de recibir daño
        StartCoroutine(DamageRecoveryRoutine());
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath() {
        if (currentHealth <= 0 && !isDead) {
            isDead = true;
            Destroy(Sword.Instance.gameObject);
            currentHealth = 0;
            UpdateHealthText(); // Asegurarse de que el texto muestre "Haz muerto!"
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            Destroy(PlayerController.Instance.gameObject);
            ScoreManager.instance.ResetScore(); // Reiniciar el puntaje cuando el jugador muere
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private IEnumerator DamageRecoveryRoutine() {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthText() {
        if (currentHealth > 0) {
            healthText.text = "" + currentHealth.ToString();
        } else {
            healthText.text = "Haz muerto!";
        }
    }
}
