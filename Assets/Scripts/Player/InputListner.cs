using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public List<GameObject> PlayerWeaponsList = new List<GameObject>();
    public List<PlayerShooting> PlayerShootingList = new List<PlayerShooting>();
    public WeaponSwitcher weaponSwitcherController;
    public KeyCode interactiveKey = KeyCode.E;

    [SerializeField] private CameraRotate cameraRotateController;
    [SerializeField] private PlayerMovement playerMovementController;
    [SerializeField] private Interactive interactive;
    [SerializeField] private PlayerMelee playerMelee;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;

    private void Start()
    {
        InitializeWeaponSwitcher();
    }

    private void Update()
    {
        HandleInput();
    }

    private void InitializeWeaponSwitcher()
    {
        if (weaponSwitcherController != null)
        {
            weaponSwitcherController.Initialize(PlayerWeaponsList);
        }
        else
        {
            Debug.LogError("Weapon Switcher Controller is not assigned!", this);
        }
    }

    private void HandleInput()
    {
        HandleCameraRotation();
        HandlePlayerMovement();
        HandleInteraction();
        HandleWeaponSwitching();
        HandleMeleeAttack();
        HandleShooting();
    }

    private void HandleCameraRotation()
    {
        if (cameraRotateController != null)
        {
            cameraRotateController.RotateCamera();
        }
        else
        {
            Debug.LogError("Camera Rotate Controller is not assigned!", this);
        }
    }

    private void HandlePlayerMovement()
    {
        if (playerMovementController != null)
        {
            playerMovementController.Move();
            playerMovementController.Sprint(Input.GetKey(sprintKey));
        }
        else
        {
            Debug.LogError("Player Movement Controller is not assigned!", this);
        }
    }

    private void HandleInteraction()
    {
        if (interactive != null)
        {
            interactive.PerformInteraction(Input.GetKeyDown(interactiveKey));
        }
        else
        {
            Debug.LogError("Interactive is not assigned!", this);
        }
    }

    private void HandleWeaponSwitching()
    {
        if (weaponSwitcherController != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f)
            {
                weaponSwitcherController.SwitchWeapon(scroll > 0f ? 1 : -1);
            }
        }
        else
        {
            Debug.LogError("Weapon Switcher Controller is not assigned!", this);
        }
    }

    private void HandleMeleeAttack()
    {
        if (playerMelee != null)
        {
            playerMelee.ExecuteAttack(Input.GetKeyDown(shootKey));
        }
        else
        {
            Debug.LogError("Player Melee is not assigned!", this);
        }
    }

    private void HandleShooting()
    {
        if (PlayerShootingList.Count > 0 && weaponSwitcherController.CurrentWeaponIndex > 0)
        {
            var currentWeapon = PlayerShootingList[weaponSwitcherController.CurrentWeaponIndex - 1];
            currentWeapon.Shoot(Input.GetKey(shootKey));
            currentWeapon.Reload(Input.GetKeyDown(reloadKey));
        }
        else
        {
            HandleShootingWarnings();
        }
    }

    private void HandleShootingWarnings()
    {
        if (PlayerShootingList.Count == 0)
        {
            Debug.LogWarning("Player Shooting List is empty!", this);
        }
        else if (weaponSwitcherController.CurrentWeaponIndex <= 0)
        {
            Debug.LogWarning("Current Weapon Index is invalid!", this);
        }
    }
}