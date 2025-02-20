using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private int damageAmount;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private Recoil recoil;

    public bool IsWeaponEquipped;

    private float timeSinceLastShot;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void Shoot(bool isKeyPressed)
    {
        if (isKeyPressed && IsWeaponEquipped && timeSinceLastShot >= timeBetweenShots)
        {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, targetLayerMask))
            {
                hit.collider.GetComponent<EnemyStatistics>()?.Damage(damageAmount);
            }

            recoil.RecoilFire();
            shootEffect?.Play();
            timeSinceLastShot = 0;
        }
    }
}