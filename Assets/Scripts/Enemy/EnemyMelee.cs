using UnityEngine;

public class EnemyMelee : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private int attackDamage;

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
        return enemyStatistics.playerTarget != null;
    }
}