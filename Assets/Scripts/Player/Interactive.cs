using TMPro;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Pick Up Settings")]
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weaponHandler;
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private InputListener inputListener;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text interactivePromptText;

    // ==============================
    // Unity Methods
    // ==============================
    private void Update()
    {
        UpdateUi();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void _Interactive(bool isKeyPressed)
    {
        if (isKeyPressed && playerCamera != null)
        {
            PerformRaycast();
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void PerformRaycast()
    {
        if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, rayDistance, targetLayerMask))
        {
            IInteractive interactiveObject = hit.collider.GetComponent<IInteractive>();
            if (interactiveObject != null)
            {
                PlayerShooting playerShoot = hit.collider.GetComponent<PlayerShooting>();
                interactiveObject.Interactive(playerStatistics, inputListener, playerShoot, weaponHandler);
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
                    interactivePromptText.text = $"Press {inputListener.interactiveKey} to Buy Item";
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