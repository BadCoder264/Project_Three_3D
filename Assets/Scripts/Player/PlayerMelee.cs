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
            return;

        animator.SetTrigger("Attack");
    }

    private void AttackLogic()
    {
        int actualDamage = attackDamage;

        if (attackPoint == null)
            return;

        Collider[] enemies = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayerMask);
        foreach (var enemy in enemies)
        {
            var enemyStatistics = enemy.GetComponent<EnemyStatistics>();
            enemyStatistics?.Damage(actualDamage);
        }
    }

    private void UpdateUI()
    {
        if (gameObject.activeSelf && ammoText != null)
        {
            ammoText.text = "";
        }
    }
}