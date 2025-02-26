using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Weapon Settings")]
    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private Recoil recoil;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text ammoText;

    // ==============================
    // Public Variables
    // ==============================
    public bool IsWeaponEquipped;

    // ==============================
    // Private Variables
    // ==============================
    private int ammo;
    private float timeSinceLastShot;
    private Vector3 initialPosition;

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        ammo = weaponSettings.MaxAmmo;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        UpdateUi();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void Shoot(bool isKeyPressed)
    {
        if (isKeyPressed && weaponSettings != null && ammo > 0 && IsWeaponEquipped && timeSinceLastShot >= weaponSettings.TimeBetweenShots)
        {
            PerformShooting();
        }
    }

    public void Reload(bool isKeyPressed)
    {
        if (isKeyPressed)
        {
            ammo = weaponSettings.MaxAmmo;
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void PerformShooting()
    {
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, weaponSettings.TargetLayerMask))
        {
            hit.collider.GetComponent<EnemyStatistics>()?.Damage(weaponSettings.DamageAmount);
        }

        recoil.RecoilFire(weaponSettings.RecoilX, weaponSettings.RecoilY, weaponSettings.RecoilZ);
        shootEffect?.Play();
        ammo--;
        timeSinceLastShot = 0;
    }

    private void UpdateUi()
    {
        if (gameObject.activeSelf && weaponSettings != null && IsWeaponEquipped)
        {
            ammoText.text = $"{ammo} / {weaponSettings.MaxAmmo}";
        }
    }
}