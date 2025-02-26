using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyStatistics : MonoBehaviour
{
    // ==============================
    // Enumerations
    // ==============================
    public enum AIState { None, Pursue, Attack }

    // ==============================
    // Serialized Fields
    // ==============================
    [field: SerializeField] public AIState CurrentState;
    public GameObject playerTarget;
    public NavMeshAgent navMeshAgent;

    [SerializeField] private int minHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int minScoreReward;
    [SerializeField] private int maxScoreReward;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private List<GameObject> enemyModels;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private MonoBehaviour enemyAttack;

    // ==============================
    // Private Fields
    // ==============================
    private IEnemyAttack enemyAttackInterface;
    private int _currentHealth;
    private int currentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            if (_currentHealth <= 0)
            {
                Death();
            }
        }
    }

    private int currentScoreReward;
    private float timeSinceLastAttack;

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        UpdateAIState();
        HandleAIState();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void Damage(int damageAmount)
    {
        playerTarget.GetComponent<PlayerStatistics>().Score += currentScoreReward;
        currentHealth -= damageAmount;
    }

    // ==============================
    // Private Methods
    // ==============================
    private void InitializeEnemy()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        int indexPlayerModel = Random.Range(0, enemyModels.Count);
        enemyModels[indexPlayerModel].SetActive(true);
        currentHealth = Random.Range(minHealth, maxHealth);
        currentScoreReward = Random.Range(minScoreReward, maxScoreReward);
        enemyAttackInterface = enemyAttack as IEnemyAttack;
    }

    private void HandleAIState()
    {
        switch (CurrentState)
        {
            case AIState.Pursue:
                enemyMovement?.MoveTowardsPlayer(playerTarget.transform.position, this);
                timeSinceLastAttack = 0;
                break;

            case AIState.Attack:
                timeSinceLastAttack += Time.deltaTime;

                if (enemyAttackInterface != null && timeSinceLastAttack >= attackCooldown)
                {
                    enemyAttackInterface.Attack(this);
                    timeSinceLastAttack = 0;
                }
                break;
        }
    }

    private void Death()
    {
        // Add logic for enemy death here
        Destroy(gameObject);
    }

    private void UpdateAIState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);
        CurrentState = distanceToPlayer < attackRange ? AIState.Attack : AIState.Pursue;
    }
}