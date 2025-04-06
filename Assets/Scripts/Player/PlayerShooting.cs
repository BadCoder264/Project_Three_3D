using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float recoilReductionPercentage;
    public bool IsWeaponEquipped;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private Recoil recoil;

    private int ammo;
    private float timeSinceLastShot;
    private bool isReload;
    private Vector3 initialPosition;

    private void Start()
    {
        if (weaponSettings == null)
            return;

        ammo = weaponSettings.MaxAmmo;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        UpdateUI();
    }

    public void Shoot(bool isKeyPressed)
    {
        if (CanShoot(isKeyPressed))
        {
            ProcessShooting();
        }
    }

    public void Reload(bool isKeyPressed)
    {
        if (isKeyPressed && ammo < weaponSettings.MaxAmmo)
        {
            StartCoroutine(PerformReload());
        }
    }

    private IEnumerator PerformReload()
    {
        animator?.SetTrigger("Reload");
        isReload = true;

        yield return new WaitForSeconds(1.75f);

        ammo = weaponSettings.MaxAmmo;
        isReload = false;
    }

    private void UpdateUI()
    {
        if (gameObject.activeSelf && weaponSettings != null && IsWeaponEquipped && ammoText != null)
        {
            ammoText.text = $"{ammo} / {weaponSettings.MaxAmmo}";
        }
    }

    private bool CanShoot(bool isKeyPressed)
    {
        return isKeyPressed && weaponSettings != null && ammo > 0 && IsWeaponEquipped &&
               timeSinceLastShot >= weaponSettings.TimeBetweenShots && !isReload;
    }

    private void ProcessShooting()
    {
        PlayerStatistics playerStatistics = FindObjectOfType<PlayerStatistics>();
        int actualDamage = weaponSettings.DamageAmount;

        if (playerStatistics.IsCraftingUpgrade)
        {
            actualDamage += Mathf.RoundToInt(actualDamage * (playerStatistics.DamageIncreasePercentage / 100f));
        }

        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, weaponSettings.TargetLayerMask))
        {
            hit.collider.GetComponent<EnemyStatistics>()?.Damage(actualDamage);
        }

        int recoilReduction = playerStatistics.RecoilReductionPercentage;

        recoil.RecoilFire(
            weaponSettings.RecoilX * (1 - recoilReduction / 100f),
            weaponSettings.RecoilY * (1 - recoilReduction / 100f),
            weaponSettings.RecoilZ * (1 - recoilReduction / 100f)
        );

        audioSource?.Play();
        animator?.SetTrigger("Shoot");
        shootEffect?.Play();
        ammo--;
        timeSinceLastShot = 0;
    }
}