using TMPro;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weaponHandler;
    [SerializeField] private InputListener inputListener;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactivePromptText;

    private void Update()
    {
        UpdateUi();
    }

    public void _Interactive(bool isKeyPressed)
    {
        if (isKeyPressed && playerCamera != null)
        {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, rayDistance, targetLayerMask))
            {
                if (hit.collider.GetComponent<ShopManager>() != null)
                {
                    hit.collider.GetComponent<ShopManager>().BuyWeapon(inputListener, hit.collider.GetComponent<PlayerShoot>(), weaponHandler);
                }

                if (hit.collider.GetComponent<WaveManager>() != null)
                {
                    hit.collider.GetComponent<WaveManager>().StartNewWave();
                }
            }
        }
    }

    private void UpdateUi()
    {
        if (playerCamera != null && interactivePromptText != null)
        {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, rayDistance, targetLayerMask))
            {
                if (hit.collider.GetComponent<ShopManager>() != null)
                {
                    interactivePromptText.text = $"Press {inputListener.interactiveKey} to Buy Weapon";
                }

                if (hit.collider.GetComponent<WaveManager>() != null)
                {
                    interactivePromptText.text = $"Press {inputListener.interactiveKey} to Start Wave";
                }
            }
            else
            {
                interactivePromptText.text = "";
            }
        }
    }
}