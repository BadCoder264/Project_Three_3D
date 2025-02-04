using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask targetLayerMask;

    public void ExecuteAttack(EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.playerTarget != null)
        {
            var playerStatistics = enemyStatistics.playerTarget.GetComponent<PlayerStatistics>();
            if (playerStatistics != null)
            {
                Debug.Log("Attack");
                playerStatistics.Damage(attackDamage);
            }
        }
    }
}