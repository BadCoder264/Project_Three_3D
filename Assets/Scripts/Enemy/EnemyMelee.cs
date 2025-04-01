using UnityEngine;

public class EnemyMelee : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRange;
    private Collider[] hitColliders;

    public void Attack(EnemyStatistics enemyStatistics)
    {
        if (IsPlayerTargetValid(enemyStatistics))
        {
            var playerStatistics = enemyStatistics.playerTarget.GetComponent<PlayerStatistics>();

            if (playerStatistics != null)
            {
                playerStatistics.Damage(attackDamage);
            }
        }
    }

    private bool IsPlayerTargetValid(EnemyStatistics enemyStatistics)
    {
        return enemyStatistics.playerTarget != null && IsPlayerInRange(enemyStatistics.playerTarget.transform);
    }

    private bool IsPlayerInRange(Transform playerTransform)
    {
        hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        foreach (var collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }
}