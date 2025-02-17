using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatistics : MonoBehaviour
{
    public enum AIState { None, Pursue, Attack }
    [field: SerializeField] public AIState CurrentState { get; private set; }
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

    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        int indexPlayerModel = Random.Range(0, enemyModels.Count);
        enemyModels[indexPlayerModel].SetActive(true);
        currentHealth = Random.Range(minHealth, maxHealth);
        currentScoreReward = Random.Range(minScoreReward, maxScoreReward);

        enemyAttackInterface = enemyAttack as IEnemyAttack;
    }

    private void Update()
    {
        UpdateAIState();

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

    public void Damage(int damageAmount)
    {
        playerTarget.GetComponent<PlayerStatistics>().Score += currentScoreReward;
        currentHealth -= damageAmount;
    }

    private void Death()
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