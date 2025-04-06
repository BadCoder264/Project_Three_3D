using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    public bool pause { get; set; }
    public List<GameObject> PlayerWeaponsList = new List<GameObject>();
    public List<PlayerShooting> PlayerShootingList = new List<PlayerShooting>();
    public WeaponSwitcher weaponSwitcherController;
    public KeyCode interactiveKey = KeyCode.E;

    [SerializeField] private CameraRotate cameraRotateController;
    [SerializeField] private PlayerMovement playerMovementController;
    [SerializeField] private Interactive interactive;
    [SerializeField] private PlayerMelee playerMelee;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

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
    }

    private void HandleInput()
    {
        if (!pause)
        {
            HandleCameraRotation();
            HandlePlayerMovement();
            HandleInteraction();
            HandleWeaponSwitching();
            HandleMeleeAttack();
            HandleShooting();
        }

        HandlePause();
    }

    private void HandleCameraRotation()
    {
        if (cameraRotateController != null)
        {
            cameraRotateController.RotateCamera();
        }
    }

    private void HandlePlayerMovement()
    {
        if (playerMovementController != null)
        {
            playerMovementController.Move();
            playerMovementController.Sprint(Input.GetKey(sprintKey));
        }
    }

    private void HandleInteraction()
    {
        if (interactive != null)
        {
            interactive.PerformInteraction(Input.GetKeyDown(interactiveKey));
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
    }

    private void HandleMeleeAttack()
    {
        if (playerMelee != null)
        {
            playerMelee.ExecuteAttack(Input.GetKeyDown(shootKey));
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
    }

    private void HandlePause()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            pause = !pause;
            pauseManager.Pause(this);
            pauseManager.gameObject.SetActive(pause);
        }
    }
}