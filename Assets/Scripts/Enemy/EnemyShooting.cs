using UnityEngine;

public class EnemyShooting : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Transform shootPoint;

    public void Attack(AudioSource audioSource)
    {
        if (Physics.Raycast(shootPoint.position, shootPoint.forward,
            out RaycastHit hit, attackDistance, targetLayerMask))
        {
            if (hit.collider.tag == "Player")
            {
                hit.collider.GetComponent<PlayerStatistics>()?.Damage(attackDamage);
                Debug.Log("Ranged attack hit player!");
            }
        }
    }
}