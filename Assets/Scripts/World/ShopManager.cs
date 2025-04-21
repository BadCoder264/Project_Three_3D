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
            productPriceDisplayText.text = $"Score: {price}";
        }
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (playerStatistics == null)
            return;

        if (inputListener == null && _TypeObject == TypeObject.Weapon)
            return;

        if (playerStatistics.Score < price)
            return;

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