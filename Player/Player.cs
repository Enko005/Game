using System;
using System.Collections;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnFlashBlink;

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private float damageRecoveryTime = 0.5f;
    [Header("Dash Settings")]
    [SerializeField] private int dashSpeed = 4;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float dashCoolDownTime = 0.25f;

    private Vector2 inputVector;

    private Rigidbody2D rb;
    private KnockBack knockBack;

    private readonly float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private bool isAlive;
    private bool isDashing;
    private int currentHealth;
    private bool canTakeDamage;
    private float initialMovingSpeed;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        knockBack = GetComponent<KnockBack>();

        initialMovingSpeed = movingSpeed;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        canTakeDamage = true;
        isAlive = true;

        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        GameInput.Instance.OnPlayerDash += GameInput_OnPlayerDash;
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (knockBack != null && knockBack.IsGettingKnockedBack)
            return;

        HandleMovement();
    }

    public bool IsAlive() => isAlive;

    public void TakeDamage(Transform damageSource, int damage)
    {
        if (canTakeDamage && isAlive)
        {
            canTakeDamage = false;
            currentHealth = Mathf.Max(0, currentHealth -= damage);
            Debug.Log(currentHealth);
            knockBack.GetKnockedBack(damageSource);

            OnFlashBlink?.Invoke(this, EventArgs.Empty);

            StartCoroutine(DamageRecoveryRoutine());
        }

        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHealth == 0 && isAlive)
        {
            isAlive = false;
            knockBack.StopKnockBackMovement();
            GameInput.Instance.DisableMovement();

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        }

    }

    private void GameInput_OnPlayerDash(object sender, System.EventArgs e) {
        Dash();
    }
    private void Dash()
    {
        if (!isDashing)
            StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
        movingSpeed *= dashSpeed;
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashTime);

        trailRenderer.emitting = false;
        movingSpeed = initialMovingSpeed;

        yield return new WaitForSeconds(dashCoolDownTime);
        isDashing = false;
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    public bool IsRunning() => isRunning;

    private void GameInput_OnPlayerAttack(object sender, EventArgs e)
    {
        if (isAlive)
        {
            ActiveWeapon.Instance.GetActiveWeapon().Attack();
        }
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    public Vector3 GetMousePosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void OnDestroy()
    {
        if (GameInput.Instance != null)
        {
            GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
            GameInput.Instance.OnPlayerDash -= GameInput_OnPlayerDash;
        }
    }
}