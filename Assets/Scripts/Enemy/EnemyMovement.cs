using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // ==============================
    // Public Methods
    // ==============================
    public void MoveTowardsPlayer(Vector3 playerPosition, EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.navMeshAgent != null)
        {
            SetDestinationToPlayer(playerPosition, enemyStatistics);
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void SetDestinationToPlayer(Vector3 playerPosition, EnemyStatistics enemyStatistics)
    {
        enemyStatistics.navMeshAgent.SetDestination(playerPosition);
    }
}