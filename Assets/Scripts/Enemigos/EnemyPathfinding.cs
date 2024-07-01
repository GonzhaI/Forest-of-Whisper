using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float followDistance = 5f; // Distancia a la que el enemigo empieza a seguir al jugador
    [SerializeField] private Vector2 randomMoveDirection = new Vector2(1, 0); // Dirección predefinida para el movimiento aleatorio cuando el jugador está muerto

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;
    private Transform playerTransform;
    private PlayerHealth playerHealth;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer
    }

    private void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void FixedUpdate() {
        if (knockback.gettingKnockedBack) { return; }

        if (playerHealth != null && !playerHealth.isDead) {
            if (Vector2.Distance(transform.position, playerTransform.position) <= followDistance) {
                MoveTo(playerTransform.position);
            }
        } else {
            MoveTo(rb.position + randomMoveDirection); // Mueve al enemigo en una dirección predefinida
        }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));

        // Ajustar flipX según la dirección del movimiento
        if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false; // Mirar hacia la derecha
        }
        else if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true; // Mirar hacia la izquierda
        }
    }

    public void MoveTo(Vector2 targetPosition) {
        moveDir = (targetPosition - rb.position).normalized;
    }
}
