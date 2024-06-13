using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class Sword : MonoBehaviour
{
    public static Sword Instance;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD = .5f;
    [SerializeField] private AudioClip swordAttackAudioClip;
    [SerializeField] private AudioSource swordAudioSource;
    
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private Weapon weapon;
    private bool attackButtonDown,  isAttacking = false;
    private Vector2 lookInput;

    private void Awake() {
        Instance = this;
        playerController = GetComponentInParent<PlayerController>();
        weapon = GetComponentInParent<Weapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
    }

    private void Update() {
        PlayerInput();
        FollowWithOffset();
        Attack();
    }

    private void PlayerInput() {
        lookInput = playerControls.Movement.Look.ReadValue<Vector2>();
    }

    private void StartAttacking() {
        attackButtonDown = true;
    }

    private void StopAttacking() {
        attackButtonDown = false;
    }

    private void Attack() {
        if (attackButtonDown && !isAttacking) {
            isAttacking = true;
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);

            if (swordAudioSource != null && swordAttackAudioClip != null) {
                swordAudioSource.clip = swordAttackAudioClip;
                swordAudioSource.Play();
            }

            StartCoroutine(AttackCDRoutine());
        }
    }

    private IEnumerator AttackCDRoutine() {
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
    }

    public void DoneAttackingAnimEvent() {
        weaponCollider.gameObject.SetActive(false);
    }

    private void FollowWithOffset() {
        if (playerController.gamepadConnected && lookInput != Vector2.zero) {
            float angle = Mathf.Atan2(0, lookInput.x) * Mathf.Rad2Deg;

            if (lookInput.x < 0) {
                weapon.transform.rotation = Quaternion.Euler(0, -180, 0);
                weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            } else {
                weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            }  
        } else if (!playerController.gamepadConnected) {
            Vector3 mousePos = Input.mousePosition;
            Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

            float angle = Mathf.Atan2(0, mousePos.x) * Mathf.Rad2Deg;

            if (mousePos.x < playerScreenPoint.x) {
                weapon.transform.rotation = Quaternion.Euler(0, -180, angle);
                weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            } else {
                weapon.transform.rotation = Quaternion.Euler(0, 0, angle);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
