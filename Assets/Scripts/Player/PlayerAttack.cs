using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private LayerMask targetLayerMask;

    private float timeSinceLastAttack;

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    public void ExecuteAttack(bool isKeyPressed)
    {
        if (isKeyPressed && timeSinceLastAttack >= timeBetweenAttack)
        {
            Collider[] enemy = Physics.OverlapSphere(transform.position, attackRange, targetLayerMask);

            if (enemy.Length > 0)
            {
                for (int i = 0; i < enemy.Length; i++)
                {
                    var enemyStatistics = enemy[i].GetComponent<EnemyStatistics>();
                    if (enemyStatistics != null)
                    {
                        enemyStatistics.Damage(attackDamage);
                    }
                }
            }

            timeSinceLastAttack = 0;
        }
    }
}