using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public void MoveTowardsPlayer(Vector3 playerPosition, EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.navMeshAgent != null)
        {
            SetDestinationToPlayer(playerPosition, enemyStatistics);
        }
    }

    private void SetDestinationToPlayer(Vector3 playerPosition, EnemyStatistics enemyStatistics)
    {
        enemyStatistics.navMeshAgent.SetDestination(playerPosition);
    }
}