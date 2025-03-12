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
        weaponSwitcherController?.Initialize(PlayerWeaponsList);
    }

    private void Update()
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
            interactive._Interactive(Input.GetKeyDown(interactiveKey));
        }
    }

    private void HandleWeaponSwitching()
    {
        if (weaponSwitcherController != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                weaponSwitcherController.SwitchWeapon(1);
            }
            else if (scroll < 0f)
            {
                weaponSwitcherController.SwitchWeapon(-1);
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
            PlayerShootingList[weaponSwitcherController.CurrentWeaponIndex - 1].Shoot(Input.GetKey(shootKey));
            PlayerShootingList[weaponSwitcherController.CurrentWeaponIndex - 1].Reload(Input.GetKeyDown(reloadKey));
        }
    }
}