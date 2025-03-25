using TMPro;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform weaponHandler;
    [SerializeField] private TMP_Text interactivePromptText;
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private InputListener inputListener;

    private void Update()
    {
        UpdateUI();
    }

    public void PerformInteraction(bool isKeyPressed)
    {
        if (isKeyPressed)
        {
            RaycastHit hit;
            if (TryGetInteractiveObject(out hit))
            {
                IInteractive interactiveObject = hit.collider.GetComponent<IInteractive>();
                if (interactiveObject != null)
                {
                    PlayerShooting playerShoot = hit.collider.GetComponent<PlayerShooting>();
                    interactiveObject.Interactive(playerStatistics, inputListener, playerShoot, weaponHandler);
                }
            }
        }
    }

    private bool TryGetInteractiveObject(out RaycastHit hit)
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned!", this);
            hit = default;
            return false;
        }

        return Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out hit, rayDistance, targetLayerMask);
    }

    private void UpdateUI()
    {
        if (playerCamera == null || interactivePromptText == null || inputListener == null)
        {
            Debug.LogError("Player Camera, Interactive Prompt Text, or Input Listener is not assigned!", this);
            return;
        }

        if (inputListener.enabled)
        {
            RaycastHit hit;
            if (TryGetInteractiveObject(out hit))
            {
                UpdatePromptText(hit);
            }
            else
            {
                interactivePromptText.text = "";
            }
        }
        else
        {
            interactivePromptText.text = "";
        }
    }

    private void UpdatePromptText(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<ShopManager>(out _))
        {
            interactivePromptText.text = $"Press {inputListener.interactiveKey} to Buy Item";
        }
        else if (hit.collider.TryGetComponent<WaveManager>(out _))
        {
            interactivePromptText.text = $"Press {inputListener.interactiveKey} to Start Wave";
        }
        else if (hit.collider.TryGetComponent<DoorManager>(out _))
        {
            interactivePromptText.text = $"Press {inputListener.interactiveKey} to Select a Location";
        }
        else if (hit.collider.TryGetComponent<UpgradeManager>(out _))
        {
            interactivePromptText.text = $"Press {inputListener.interactiveKey} to Upgrade the Room";
        }
        else
        {
            interactivePromptText.text = "";
        }
    }
}