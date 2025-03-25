using TMPro;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private Animator animator;

    private float timeSinceLastAttack;

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        UpdateUI();
    }

    public void ExecuteAttack(bool isKeyPressed)
    {
        if (isKeyPressed && CanAttack())
        {
            TriggerAttackAnimation();
            Invoke("AttackLogic", 0.3f);
            timeSinceLastAttack = 0;
        }
    }

    private bool CanAttack()
    {
        return timeSinceLastAttack >= timeBetweenAttack;
    }

    private void TriggerAttackAnimation()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!", this);
            return;
        }

        animator.SetTrigger("Attack");
    }

    private void AttackLogic()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned!", this);
            return;
        }

        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayerMask);
        foreach (var enemy in enemies)
        {
            var enemyStatistics = enemy.GetComponent<EnemyStatistics>();
            enemyStatistics?.Damage(attackDamage);
        }
    }

    private void UpdateUI()
    {
        if (gameObject.activeSelf)
        {
            if (ammoText != null)
            {
                ammoText.text = "";
            }
            else
            {
                Debug.LogError("Ammo Text is not assigned!", this);
            }
        }
    }
}