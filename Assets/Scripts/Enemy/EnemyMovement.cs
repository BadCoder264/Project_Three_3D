using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public void MoveTowardsPlayer(Vector3 playerPosition, EnemyStatistics enemyStatistics)
    {
        if (IsNavMeshAgentAvailable(enemyStatistics))
        {
            enemyStatistics.navMeshAgent.SetDestination(playerPosition);
        }
    }

    private bool IsNavMeshAgentAvailable(EnemyStatistics enemyStatistics)
    {
        return enemyStatistics.navMeshAgent != null;
    }
}