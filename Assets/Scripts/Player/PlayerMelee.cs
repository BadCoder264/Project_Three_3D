using TMPro;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Weapon Settings")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animator;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text ammoText;

    // ==============================
    // Private Variables
    // ==============================
    private float timeSinceLastAttack;

    // ==============================
    // Unity Methods
    // ==============================
    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        UpdateUi();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void ExecuteAttack(bool isKeyPressed)
    {
        if (isKeyPressed && timeSinceLastAttack >= timeBetweenAttack)
        {
            PerformAttack();
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void PerformAttack()
    {
        animator.SetTrigger("Attack");
        Invoke("AttackLogic", 0.3f);

        timeSinceLastAttack = 0;
    }

    private void AttackLogic()
    {
        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayerMask);

        if (enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                var enemyStatistics = enemies[i].GetComponent<EnemyStatistics>();
                if (enemyStatistics != null)
                {
                    enemyStatistics.Damage(attackDamage);
                }
            }
        }
    }

    private void UpdateUi()
    {
        if (gameObject.activeSelf)
        {
            ammoText.text = "";
        }
    }
}