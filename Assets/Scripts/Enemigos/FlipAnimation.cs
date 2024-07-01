using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool facingRight = true; // Asume que el prefab inicialmente mira hacia la derecha

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Flip();
    }

    public void Flip()
    {
        // Invertir flipX
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
