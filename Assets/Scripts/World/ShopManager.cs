using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour, IInteractive
{
    public enum TypeObject { Weapon, FirstAid }

    [field: SerializeField] public TypeObject _TypeObject;
    [SerializeField] private int price;
    [SerializeField] private GameObject product;
    [SerializeField] private TMP_Text productPriceDisplayText;

    private const int HealingAmount = 8;

    private void Start()
    {
        if (productPriceDisplayText)
        {
            productPriceDisplayText.text = $"Price: {price}";
        }
        else
        {
            Debug.LogError("Product Price Display Text is not assigned!", this);
        }
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (playerStatistics == null)
        {
            Debug.LogError("Player Statistics is not assigned!", this);
            return;
        }

        if (inputListener == null && _TypeObject == TypeObject.Weapon)
        {
            Debug.LogError("Input Listener is not assigned for weapon purchase!", this);
            return;
        }

        if (playerStatistics.Score < price)
        {
            Debug.LogWarning("Not enough score to purchase the item!", this);
            return;
        }

        if (_TypeObject == TypeObject.Weapon)
        {
            PurchaseWeapon(inputListener, playerShoot, weaponHandler);
        }
        else if (_TypeObject == TypeObject.FirstAid)
        {
            PurchaseFirstAid(playerStatistics);
        }

        playerStatistics.Score -= price;
    }

    private void PurchaseWeapon(InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (inputListener != null && playerShoot != null)
        {
            inputListener.PlayerWeaponsList.Add(gameObject);
            inputListener.PlayerShootingList.Add(playerShoot);
            inputListener.weaponSwitcherController.CurrentWeaponIndex = inputListener.PlayerShootingList.Count;

            playerShoot.IsWeaponEquipped = true;

            foreach (Transform child in weaponHandler)
            {
                child.gameObject.SetActive(false);
            }

            transform.SetParent(weaponHandler);
            CompletePurchase();
        }
        else
        {
            Debug.LogError("Input Listener or Player Shooting is not assigned!", this);
        }
    }

    private void PurchaseFirstAid(PlayerStatistics playerStatistics)
    {
        playerStatistics.Healing(HealingAmount);
    }

    private void CompletePurchase()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        else
        {
            Debug.LogError("BoxCollider is not attached to the ShopManager!", this);
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(true);
        Destroy(this);

        if (product != null)
        {
            Destroy(product);
        }
    }
}