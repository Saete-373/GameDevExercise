using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IDamagable
{
    [Header("Movement Settings")]
    float _horizontalInput;
    float _walkSpeed = 12f;
    float _runSpeed = 20f;
    float _speed;
    float _jumpingPower = 25f;
    bool _isFacingRight = true;
    bool _isGrounded;
    float _groundCheckRadius = 0.2f;

    [Header("Status")]
    public float MaxHealth = 100;
    public float CurrentHealth { get; private set; }

    bool _isDead;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;
    Coroutine _OnAttack;

    [Header("References")]
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _anim;
    [SerializeField] Transform _groundCheck;
    [SerializeField] LayerMask _groundLayer;


    void Start()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    void Update()
    {
        GetInput();
        UpdateGrounded();
        UpdateAnimation();
        Flip();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        if (_isDead)
            return;

        _horizontalInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            _horizontalInput = -1f;
        else if (Keyboard.current.dKey.isPressed)
            _horizontalInput = 1f;

        _speed = Keyboard.current.leftShiftKey.isPressed
            ? _runSpeed
            : _walkSpeed;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && _isGrounded)
        {
            Jump();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (EquipmentManager.Instance.EquippedItem == null)
                return;

            if (_OnAttack != null)
                return;

            _OnAttack = StartCoroutine(Attack());
        }

    }

    #region Movement

    void Move()
    {
        if (_isDead)
            return;

        _rb.linearVelocity = new Vector2(_horizontalInput * _speed, _rb.linearVelocity.y);
    }

    void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpingPower);
    }

    void UpdateGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundCheckRadius,
            _groundLayer
        );
    }

    void Flip()
    {
        if (_horizontalInput == 0f)
            return;

        if ((_isFacingRight && _horizontalInput < 0f) ||
            (!_isFacingRight && _horizontalInput > 0f))
        {
            _isFacingRight = !_isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    #endregion

    #region Health

    public void TakeDamage(float damage, GameObject damageSource)
    {
        if (_isDead)
            return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            _anim.SetTrigger("hurt");
        }
    }

    void Die()
    {
        if (_isDead)
            return;

        _isDead = true;

        _horizontalInput = 0f;
        _rb.linearVelocity = Vector2.zero;

        _anim.SetTrigger("die");
    }

    // Animation Event
    public void OnDeathAnimationFinished()
    {
        OnDeath?.Invoke();
    }

    #endregion

    #region Attack
    IEnumerator Attack()
    {
        _anim.SetTrigger("attack");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        FaceDirection(direction.x);

        EquipmentManager.Instance.UseEquippedItem(direction, gameObject);

        yield return new WaitForSeconds(0.5f);

        _OnAttack = null;
    }



    void FaceDirection(float directionX)
    {
        if (directionX == 0f)
            return;

        if ((_isFacingRight && directionX < 0f) ||
            (!_isFacingRight && directionX > 0f))
        {
            _isFacingRight = !_isFacingRight;

            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    #endregion

    void UpdateAnimation()
    {
        bool isMoving = !_isDead && _isGrounded && _horizontalInput != 0f;

        _anim.SetBool("isJump", !_isGrounded);
        _anim.SetBool("isRun", isMoving);

        if (isMoving)
        {
            _anim.SetFloat(
                "speedMultiplier",
                _speed / _walkSpeed
            );
        }
    }
}