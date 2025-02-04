using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public void MoveTowardsPlayer(Vector3 playerPosition, EnemyStatistics enemyStatistics)
    {
        if (enemyStatistics.navMeshAgent != null)
        {
            enemyStatistics.navMeshAgent.SetDestination(playerPosition);
        }
    }
}