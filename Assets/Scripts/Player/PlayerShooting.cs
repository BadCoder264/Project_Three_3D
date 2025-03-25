using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public float recoilReductionPercentage;
    public bool IsWeaponEquipped;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private TMP_Text ammoText;
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
        {
            Debug.LogError("Weapon Settings is not assigned!", this);
            return;
        }

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
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!", this);
            yield break;
        }

        animator.SetTrigger("Reload");
        isReload = true;

        yield return new WaitForSeconds(1.4f);

        ammo = weaponSettings.MaxAmmo;
        isReload = false;
    }

    private void UpdateUI()
    {
        if (gameObject.activeSelf && weaponSettings != null && IsWeaponEquipped && ammoText != null)
        {
            ammoText.text = $"{ammo} / {weaponSettings.MaxAmmo}";
        }
        else if (ammoText == null)
        {
            Debug.LogError("Ammo Text is not assigned!", this);
        }
    }

    private bool CanShoot(bool isKeyPressed)
    {
        return isKeyPressed && weaponSettings != null && ammo > 0 && IsWeaponEquipped &&
               timeSinceLastShot >= weaponSettings.TimeBetweenShots && !isReload;
    }

    private void ProcessShooting()
    {
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, weaponSettings.TargetLayerMask))
        {
            hit.collider.GetComponent<EnemyStatistics>()?.Damage(weaponSettings.DamageAmount);
        }

        PlayerStatistics playerStatistics = FindObjectOfType<PlayerStatistics>();
        int recoilReduction = playerStatistics.RecoilReductionPercentage;

        recoil.RecoilFire(
            weaponSettings.RecoilX * (1 - recoilReduction / 100),
            weaponSettings.RecoilY * (1 - recoilReduction / 100),
            weaponSettings.RecoilZ * (1 - recoilReduction / 100)
        );

        animator?.SetTrigger("Shoot");
        shootEffect?.Play();
        ammo--;
        timeSinceLastShot = 0;
    }
}