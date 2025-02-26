using UnityEngine;

public class EnemyMelee : MonoBehaviour, IEnemyAttack
{
    // ==============================
    // Serialized Fields
    // ==============================
    [SerializeField] private int attackDamage;

    // ==============================
    // Public Methods
    // ==============================
    public void Attack(EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.playerTarget != null)
        {
            ApplyDamageToPlayer(enemyStatistics.playerTarget);
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void ApplyDamageToPlayer(GameObject playerTarget)
    {
        var playerStatistics = playerTarget.GetComponent<PlayerStatistics>();

        if (playerStatistics != null)
        {
            playerStatistics.Damage(attackDamage);
        }
    }
}