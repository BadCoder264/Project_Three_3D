using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public bool IsWeaponEquipped;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponSettings weaponSettings;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem shootEffect;
    [SerializeField] private Recoil recoil;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text ammoText;

    private int ammo;
    private float timeSinceLastShot;
    private Vector3 initialPosition;

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

    public void Shoot(bool isKeyPressed)
    {
        if (isKeyPressed && weaponSettings != null && ammo > 0 && IsWeaponEquipped && timeSinceLastShot >= weaponSettings.TimeBetweenShots)
        {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, weaponSettings.TargetLayerMask))
            {
                hit.collider.GetComponent<EnemyStatistics>()?.Damage(weaponSettings.DamageAmount);
            }

            recoil.RecoilFire(weaponSettings.RecoilX, weaponSettings.RecoilY, weaponSettings.RecoilZ);
            animator.SetTrigger("Shoot");
            shootEffect?.Play();
            ammo--;
            timeSinceLastShot = 0;
        }
    }

    public void Reload(bool isKeyPressed)
    {
        if (isKeyPressed)
        {
            StartCoroutine(PerformReload());
        }
    }

    private IEnumerator PerformReload()
    {
        animator.SetTrigger("Reload");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        ammo = weaponSettings.MaxAmmo;
    }

    private void UpdateUi()
    {
        if (gameObject.activeSelf && weaponSettings != null && IsWeaponEquipped)
        {
            ammoText.text = $"{ammo} / {weaponSettings.MaxAmmo}";
        }
    }
}