using UnityEngine;

public class EnemyShooting : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform startShoot;

    public void Attack(EnemyStatistics enemyStatistics)
    {
        if (IsPlayerTargetValid(enemyStatistics))
        {
            PerformRaycastAttack();
        }
    }

    private bool IsPlayerTargetValid(EnemyStatistics enemyStatistics)
    {
        return enemyStatistics.playerTarget != null;
    }

    private void PerformRaycastAttack()
    {
        if (Physics.Raycast(startShoot.position, startShoot.forward, out RaycastHit hit, Mathf.Infinity, targetLayerMask))
        {
            var playerStatistics = hit.collider.GetComponent<PlayerStatistics>();
            playerStatistics?.Damage(attackDamage);
        }
    }
}