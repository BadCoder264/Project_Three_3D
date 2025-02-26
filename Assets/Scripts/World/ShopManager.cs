using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour, IInteractive
{
    // ==============================
    // Serialized Fields
    // ==============================
    [field: SerializeField] public TypeObject _TypeObject;
    [SerializeField] private int price;
    [SerializeField] private GameObject product;
    [SerializeField] private TMP_Text productPriceDisplayText;

    // ==============================
    // Public Enums
    // ==============================
    public enum TypeObject { Weapon, FirstAid }

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        UpdatePriceDisplay();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (playerStatistics.Score >= price)
        {
            if (_TypeObject == TypeObject.Weapon)
            {
                EquipWeaponIfPossible(inputListener, playerShoot, weaponHandler);
            }
            else if (_TypeObject == TypeObject.FirstAid)
            {
                HealPlayer(playerStatistics);
            }

            DeductScore(playerStatistics);
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void UpdatePriceDisplay()
    {
        if (productPriceDisplayText != null)
        {
            productPriceDisplayText.text = $"Price: {price}";
        }
    }

    private void EquipWeaponIfPossible(InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (playerShoot != null && inputListener != null)
        {
            EquipWeapon(inputListener, playerShoot, weaponHandler);
            CompletePurchase();
        }
    }

    private void HealPlayer(PlayerStatistics playerStatistics)
    {
        if (playerStatistics != null)
        {
            playerStatistics.Healing(8);
        }
    }

    private void DeductScore(PlayerStatistics playerStatistics)
    {
        playerStatistics.Score -= price;
    }

    private void EquipWeapon(InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        inputListener.PlayerWeaponsList.Add(gameObject);
        inputListener.PlayerShootingList.Add(playerShoot);
        inputListener.weaponSwitcherController.CurrentWeaponIndex = inputListener.PlayerShootingList.Count;

        playerShoot.IsWeaponEquipped = true;

        if (weaponHandler != null)
        {
            foreach (Transform child in weaponHandler)
            {
                child.gameObject.SetActive(false);
            }

            transform.SetParent(weaponHandler);
        }
    }

    private void CompletePurchase()
    {
        GetComponent<BoxCollider>().enabled = false;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
        Destroy(this);
        Destroy(product);
    }
}