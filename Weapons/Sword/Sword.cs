using System;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damageAmount = 2;
    public event EventHandler OnSwordSwing;

    private PolygonCollider2D polygonCollider2D;
    private void Awake()
    {
        polygonCollider2D = GetComponent<PolygonCollider2D>();

    }
    private void Start()
    {
        AttackColliderOff();

    }
    public void Attack()
    {
        AttackColliderOffOn();
        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    public void AttackColliderOff()
    {
        polygonCollider2D.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity))
        {
            enemyEntity.TakeDamage(damageAmount);
        }
    }

    private void AttackColliderOn()
    {
        polygonCollider2D.enabled = true;
    }
    private void AttackColliderOffOn()
    {
        AttackColliderOff();
        AttackColliderOn();
    }
}

