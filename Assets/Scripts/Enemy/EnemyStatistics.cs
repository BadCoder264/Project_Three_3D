using UnityEngine;
using UnityEngine.AI;

public class EnemyStatistics : MonoBehaviour
{
    public enum AIState { None, Pursue, Attack }
    [field: SerializeField] public AIState CurrentState { get; private set; }
    public GameObject playerTarget;
    public NavMeshAgent navMeshAgent;

    [SerializeField] private int maxHealth;
    [SerializeField] private int scoreReward;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAttack enemyAttack;

    private int currentHealth;
    private int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }
    }
    private float timeSinceLastAttack;

    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    private void Update()
    {
        UpdateAIState();

        switch (CurrentState)
        {
            case AIState.Pursue:
                enemyMovement?.MoveTowardsPlayer(playerTarget.transform.position, this);
                break;

            case AIState.Attack:
                timeSinceLastAttack += Time.deltaTime;

                if (enemyAttack != null && timeSinceLastAttack >= attackCooldown)
                {
                    enemyAttack.ExecuteAttack(this);
                    timeSinceLastAttack = 0;
                }
                break;
        }
    }

    public void Damage(int damageAmount)
    {
        playerTarget.GetComponent<PlayerStatistics>().Score += scoreReward;
        CurrentHealth -= damageAmount;
    }

    private void HandleDeath()
    {
        // Добавьте логику для смерти врага здесь
        Destroy(gameObject);
    }

    private void UpdateAIState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);

        if (distanceToPlayer < attackRange)
        {
            CurrentState = AIState.Attack;
        }
        else
        {
            CurrentState = AIState.Pursue;
        }
    }
}