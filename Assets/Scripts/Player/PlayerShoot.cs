using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public bool IsWeaponEquipped { get; set; }

    [Header("Weapon Settings")]
    [SerializeField] private int damageAmount;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem shootEffect;

    private float timeSinceLastShot;

    private void Update()
    {
        if (timeSinceLastShot < timeBetweenShots)
        {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    public void Shoot(bool isKeyPressed)
    {
        if (isKeyPressed && IsWeaponEquipped && timeSinceLastShot >= timeBetweenShots)
        {
            RaycastHit hit;

            if (playerCamera != null)
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayerMask))
                {
                    EnemyStatistics enemyStats = hit.collider.GetComponent<EnemyStatistics>();
                    if (enemyStats != null)
                    {
                        enemyStats.Damage(damageAmount);
                    }
                }
            }

            if (shootEffect != null)
            {
                shootEffect.Play();
            }

            timeSinceLastShot = 0;
        }
    }

    public void EquipWeapon(bool isEquipped)
    {
        IsWeaponEquipped = isEquipped;
    }
}
