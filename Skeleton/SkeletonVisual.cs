using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SkeletonVisual : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyEntity enemyEntity;
    [SerializeField] private GameObject enemyShadow;
    private const string ATTACK = "Attack";


    private const string IsRunning = "IsRunning";
    private const string TAKEHIT = "TakeHit";
    private const string IS_DIE = "isDie";
    private const string ChasingSpeedMultiplier = "ChasingSpeedMultiplier";


    SpriteRenderer spriteRender;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        enemyAI.OnEnemyAttack += enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit += enemyEntity_OnTakeHit;
        enemyEntity.OnDeath += enemyEntity_OnDeath;
    }
    private void enemyEntity_OnDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        spriteRender.sortingOrder = -1;
        enemyShadow.SetActive(false);
    }
    private void enemyEntity_OnTakeHit(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TAKEHIT);
    }
    private void Update()
    {
        animator.SetBool(IsRunning, enemyAI.IsRunning);
        animator.SetFloat(ChasingSpeedMultiplier, enemyAI.GetRoamingAnimationSpeed());
    }
    private void TrigerAttackAnimationTurnOff()
    {
        enemyEntity.PolygonColliderOff();
    }
    private void TrigerAttackAnimationTurnOn()
    {
        enemyEntity.PolygonColliderOn();
    }

    private void enemyAI_OnEnemyAttack(object sender, System.EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }

    private void OnDestroy()
    {
        enemyAI.OnEnemyAttack -= enemyAI_OnEnemyAttack;
        enemyEntity.OnTakeHit -= enemyEntity_OnTakeHit;
        enemyEntity.OnDeath -= enemyEntity_OnDeath;
    }
}
