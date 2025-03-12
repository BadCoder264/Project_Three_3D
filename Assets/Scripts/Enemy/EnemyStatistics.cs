using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyStatistics : MonoBehaviour
{
    public enum AIState { None, Pursue, Attack }
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
    [SerializeField] private Animator animator;
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
        InitializeEnemy();
    }

    private void Update()
    {
        UpdateAIState();
        HandleAIState();
    }

    public void Damage(int damageAmount)
    {
        playerTarget.GetComponent<PlayerStatistics>().Score += currentScoreReward;
        currentHealth -= damageAmount;
    }

    private void InitializeEnemy()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        int indexPlayerModel = Random.Range(0, enemyModels.Count);
        enemyModels[indexPlayerModel].SetActive(true);
        currentHealth = Random.Range(minHealth, maxHealth);
        currentScoreReward = Random.Range(minScoreReward, maxScoreReward);
        enemyAttackInterface = enemyAttack as IEnemyAttack;
        int indexWalk = Random.Range(0, 2);
        animator = enemyModels[indexPlayerModel].GetComponent<Animator>();
        animator.SetInteger("IndexWalk", indexWalk);
        timeSinceLastAttack = attackCooldown;
    }

    private void HandleAIState()
    {
        switch (CurrentState)
        {
            case AIState.Pursue:
                animator.SetBool("Walk", true);
                enemyMovement?.MoveTowardsPlayer(playerTarget.transform.position, this);
                timeSinceLastAttack = attackCooldown;
                break;

            case AIState.Attack:
                animator.SetBool("Walk", false);
                LookAtPlayer();
                timeSinceLastAttack += Time.deltaTime;

                if (enemyAttackInterface != null && timeSinceLastAttack >= attackCooldown)
                {
                    animator.SetTrigger("Attack");
                    Invoke("PerformAttackAfterDelay", 1f);
                    timeSinceLastAttack = 0;
                }

                break;
        }
    }

    private void PerformAttackAfterDelay()
    {
        enemyAttackInterface.Attack(this);
    }

    private void LookAtPlayer()
    {
        Vector3 directionToPlayer = playerTarget.transform.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
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