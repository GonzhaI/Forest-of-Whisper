using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    public static PlayerController Instance;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private AudioClip dashAudioClip;
    [SerializeField] private AudioSource dashAudioSource;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Vector2 lookInput;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    public bool gamepadConnected = false;

    // Referencia al script PlayerHealth
    private PlayerHealth playerHealth;

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();

        // Obtener la referencia a PlayerHealth
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();

        startingMoveSpeed = moveSpeed;

        CheckForGamepad();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();

        // Verificar si el jugador puede moverse antes de aplicar el movimiento
        if (!playerHealth.isDead)
        {
            Move();
        }
        else
        {
            DisableMovement();
        }
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        lookInput = playerControls.Movement.Look.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.gettingKnockedBack) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        if (gamepadConnected && lookInput != Vector2.zero)
        {
            if (lookInput.x < 0)
            {
                mySpriteRender.flipX = true;
                facingLeft = true;
            }
            else if (lookInput.x > 0)
            {
                mySpriteRender.flipX = false;
                facingLeft = false;
            }
        }
        else if (!gamepadConnected)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

            if (mousePos.x < playerScreenPoint.x)
            {
                mySpriteRender.flipX = true;
                facingLeft = true;
            }
            else
            {
                mySpriteRender.flipX = false;
                facingLeft = false;
            }
        }
    }

    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            
            if (dashAudioSource != null && dashAudioClip != null)
            {
                dashAudioSource.clip = dashAudioClip;
                dashAudioSource.Play();
            }

            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

    private void CheckForGamepad()
    {
        gamepadConnected = Gamepad.current != null;
        SetCursorState();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added)
            {
                gamepadConnected = true;
            }
            else if (change == InputDeviceChange.Removed)
            {
                gamepadConnected = false;
            }
            SetCursorState();
        }
    }

    private void SetCursorState()
    {
        if (gamepadConnected)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void DisableMovement()
    {
        // Desactivar input del jugador
        playerControls.Disable();

        // Desactivar el mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Activar isTrigger en el collider del jugador
        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.isTrigger = true;
        }
    }

    public void EnableMouse()
    {
        // Habilitar el mouse
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
