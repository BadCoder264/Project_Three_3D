using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Item Settings")]
    [SerializeField] private int price;
    [SerializeField] private GameObject product;
    [SerializeField] private PlayerStatistics playerStatistics;

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

    public void BuyWeapon(InputListener inputListener, PlayerShoot playerShoot, Transform weaponHandler)
    {
        if (playerStatistics.Score < price) return;

        if (playerShoot != null && inputListener != null)
        {
            EquipWeapon(inputListener, playerShoot, weaponHandler);
            playerStatistics.Score -= price;
            CompletePurchase();
        }
    }

    private void EquipWeapon(InputListener inputListener, PlayerShoot playerShoot, Transform weaponHandler)
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