using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Item Settings")]
    [SerializeField] private int price;
    [SerializeField] private Transform product;
    [SerializeField] private PlayerStatistics playerStatistics;

    public void Buy()
    {
        if (playerStatistics.Score >= price)
        {
            playerStatistics.Score -= price;
            product.transform.parent = null;
        }
    }
}