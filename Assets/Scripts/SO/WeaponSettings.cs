using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ProjectOne3D/WeaponSettings")]
public class WeaponSettings : ScriptableObject
{
    // ==============================
    // Weapon Attributes
    // ==============================
    public int DamageAmount;
    public int MaxAmmo;
    public float RecoilX;
    public float RecoilY;
    public float RecoilZ;
    public float TimeBetweenShots;

    // ==============================
    // Layer Settings
    // ==============================
    public LayerMask TargetLayerMask;
}