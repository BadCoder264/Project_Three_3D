using UnityEngine;

public class EnemyShooting : MonoBehaviour, IEnemyAttack
{
    // ==============================
    // Serialized Fields
    // ==============================
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform startShoot;

    // ==============================
    // Public Methods
    // ==============================
    public void Attack(EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.playerTarget != null)
        {
            TryHitPlayer();
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void TryHitPlayer()
    {
        if (Physics.Raycast(startShoot.position, startShoot.forward, out RaycastHit hit, Mathf.Infinity, targetLayerMask))
        {
            var playerStatistics = hit.collider.GetComponent<PlayerStatistics>();
            playerStatistics?.Damage(attackDamage);
        }
    }
}