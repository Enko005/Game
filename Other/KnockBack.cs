using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour
{
    [SerializeField] private float knockBackForce = 8f;
    [SerializeField] private float knockBackDuration = 0.3f;

    private float knockBackTimer;
    private Rigidbody2D rb;
    private bool isGettingKnockedBack;

    public bool IsGettingKnockedBack => isGettingKnockedBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isGettingKnockedBack)
        {
            knockBackTimer -= Time.deltaTime;
            if (knockBackTimer <= 0)
            {
                StopKnockBackMovement();
            }
        }
    }

    public void GetKnockedBack(Transform damageSource)
    {
        if (damageSource == null) return;

        isGettingKnockedBack = true;
        knockBackTimer = knockBackDuration;

        Vector2 direction = (transform.position - damageSource.position).normalized;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * knockBackForce, ForceMode2D.Impulse);
        }
    }

    public void StopKnockBackMovement()
    {
        isGettingKnockedBack = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}