using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{
    
    public float speed = 3.0f;
    public float attackRange = 2.0f;
    public int attackDamage = 1;
    private Transform player;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;

    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (isAttacking) return;

        animator.Play("d_walk");
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Voltear el sprite del Demon para mirar al jugador
        if (player.position.x > transform.position.x)
        {
            // Mirar hacia la derecha
            transform.localScale = new Vector3(-4, 4, 4);
        }
        else
        {
            // Mirar hacia la izquierda
            transform.localScale = new Vector3(4, 4, 4);
        }
    }

    void AttackPlayer()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.Play("d_cleave");

        // Aquí puedes añadir el código para el daño al jugador.
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.0f); // Duración del ataque.
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth.TakeDamage(damage);

        if (enemyHealth.GetCurrentHealth() <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.Play("death");
        
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {   
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
