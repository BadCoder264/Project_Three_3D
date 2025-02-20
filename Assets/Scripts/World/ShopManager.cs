using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour, IInteractive
{
    public enum TypeObject { Weapon, FirstAid }
    [field: SerializeField] public TypeObject _TypeObject;

    [Header("Shop Item Settings")]
    [SerializeField] private int price;
    [SerializeField] private GameObject product;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text productPriceDisplayText;

    private void Start()
    {
        UpdatePriceDisplay();
    }

    private void UpdatePriceDisplay()
    {
        if (productPriceDisplayText != null)
        {
            productPriceDisplayText.text = $"Price: {price}";
        }
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (playerStatistics.Score >= price)
        {
            if (_TypeObject == TypeObject.Weapon)
            {
                if (playerShoot != null && inputListener != null)
                {
                    EquipWeapon(inputListener, playerShoot, weaponHandler);
                    CompletePurchase();
                }
            }
            else if (_TypeObject == TypeObject.FirstAid)
            {
                if (playerStatistics != null)
                {
                    playerStatistics.Healing(8);
                }
            }

            playerStatistics.Score -= price;
        }
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