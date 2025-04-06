using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class EnemyStatistics : MonoBehaviour
{
    public enum AIState { None, Pursue, Attack }
    [field: SerializeField] public AIState CurrentState;
    public int currentHealth
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
    public GameObject playerTarget;
    public NavMeshAgent navMeshAgent;

    [SerializeField] private int minHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int minScoreReward;
    [SerializeField] private int maxScoreReward;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackCooldown;
    [SerializeField] private List<GameObject> enemyModels;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClipsDamage;
    [SerializeField] private List<AudioClip> audioClipsDeath;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private MonoBehaviour enemyAttack;

    private int _currentHealth;
    private int currentScoreReward;
    private float timeSinceLastAttack;
    private float dissolveRate = 0.0125f;
    private float refreshRate = 0.025f;
    private bool isDead = false;
    private bool isPlaying = false;
    private IEnemyAttack enemyAttackInterface;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private AudioClip currentClip;
    private Animator animator;
    private Material[] materials;

    private void Start()
    {
        InitializeEnemy();
    }

    private void Update()
    {
        if (isDead || playerTarget == null)
            return;

        UpdateAIState();
        HandleAIState();
    }

    public void Damage(int damageAmount)
    {
        if (isDead)
            return;

        currentHealth -= damageAmount;
        HandleDamageSounds(audioClipsDamage);

        if (playerTarget != null)
        {
            playerTarget.GetComponent<PlayerStatistics>().Score += currentScoreReward;
        }
    }

    private void InitializeEnemy()
    {
        int indexPlayerModel = Random.Range(0, enemyModels.Count);
        ActivateEnemyModel(indexPlayerModel);
        currentHealth = Random.Range(minHealth, maxHealth);
        currentScoreReward = Random.Range(minScoreReward, maxScoreReward);
        timeSinceLastAttack = attackCooldown;
        playerTarget = GameObject.FindGameObjectWithTag("Player");

        if (playerTarget == null)
            return;

        skinnedMeshRenderer = enemyModels[indexPlayerModel].GetComponentInChildren<SkinnedMeshRenderer>();

        if (skinnedMeshRenderer != null)
        {
            materials = skinnedMeshRenderer.materials;
        }

        animator = enemyModels[indexPlayerModel].GetComponent<Animator>();
        enemyAttackInterface = enemyAttack as IEnemyAttack;
    }

    private void ActivateEnemyModel(int index)
    {
        for (int i = 0; i < enemyModels.Count; i++)
        {
            enemyModels[i].SetActive(i == index);
        }
    }

    private void HandleAIState()
    {
        if (isDead)
            return;

        switch (CurrentState)
        {
            case AIState.Pursue:
                PursuePlayer();
                break;

            case AIState.Attack:
                AttackPlayer();
                break;
        }
    }

    private void PursuePlayer()
    {
        animator.SetBool("Walk", true);
        enemyMovement?.MoveTowardsPlayer(playerTarget.transform.position, audioSource, this);
        timeSinceLastAttack = attackCooldown;
    }

    private void AttackPlayer()
    {
        animator.SetBool("Walk", false);
        LookAtPlayer();
        timeSinceLastAttack += Time.deltaTime;

        if (enemyAttackInterface != null && timeSinceLastAttack >= attackCooldown)
        {
            animator.SetTrigger("Attack");
            enemyAttackInterface.Attack(audioSource);
            StartCoroutine(AttackCooldown());
            timeSinceLastAttack = 0;
        }
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
        if (isDead)
            return;

        isDead = true;

        GetComponent<Collider>().enabled = false;
        animator.SetTrigger("Dying");
        HandleDamageSounds(audioClipsDeath);

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }

        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }

        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }

        StartCoroutine(DissolveEffect());
        Destroy(gameObject, 1.6f);
    }

    private void HandleDamageSounds(List<AudioClip> audioClips)
    {
        if (audioSource == null || audioClipsDamage == null || audioClipsDamage.Count == 0)
            return;

        if (!isPlaying)
        {
            PlayRandomSound(audioClips);
        }
        else if (!audioSource.isPlaying)
        {
            isPlaying = false;
            PlayRandomSound(audioClips);
        }
    }

    private void PlayRandomSound(List<AudioClip> audioClips)
    {
        int randomIndex = Random.Range(0, audioClips.Count);
        currentClip = audioClips[randomIndex];

        audioSource.clip = currentClip;
        audioSource.Play();
        isPlaying = true;
    }

    private void UpdateAIState()
    {
        if (isDead)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTarget.transform.position);
        CurrentState = distanceToPlayer < attackRange ? AIState.Attack : AIState.Pursue;
    }

    IEnumerator DissolveEffect()
    {
        if (materials != null && materials.Length > 0)
        {
            float counter = 0;

            while (materials[0].GetFloat("_DissolveAmmount") < 1)
            {
                counter += dissolveRate;

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].SetFloat("_DissolveAmmount", counter);
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        navMeshAgent.isStopped = true;

        yield return new WaitForSeconds(1.4f);

        if (!isDead && navMeshAgent != null)
        {
            navMeshAgent.isStopped = false;
        }
    }
}