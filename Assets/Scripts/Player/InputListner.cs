using System.Collections.Generic;
using UnityEngine;

public class InputListener : MonoBehaviour
{
    [SerializeField] private CameraRotate cameraRotateController;
    [SerializeField] private PlayerMovement playerMovementController;
    [SerializeField] private Interactive interactive;
    public List<GameObject> PlayerWeaponsList = new List<GameObject>();
    [SerializeField] private PlayerAttack playerAttack;
    public List<PlayerShoot> PlayerShootingList = new List<PlayerShoot>();
    public WeaponSwitcher weaponSwitcherController;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode interactiveKey = KeyCode.E;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

    private void Start()
    {
        weaponSwitcherController?.Initialize(PlayerWeaponsList);
    }

    private void Update()
    {
        if (cameraRotateController != null)
        {
            cameraRotateController.RotateCamera();
        }

        if (playerMovementController != null)
        {
            playerMovementController.Move();
            playerMovementController.Sprint(Input.GetKey(sprintKey));
        }

        if (interactive != null)
        {
            interactive._Interactive(Input.GetKeyDown(interactiveKey));
        }

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

        if (playerAttack != null)
        {
            playerAttack.ExecuteAttack(Input.GetKeyDown(shootKey));
        }

        if (PlayerShootingList.Count > 0 &&
            weaponSwitcherController.CurrentWeaponIndex > 0)
        {
            PlayerShootingList[weaponSwitcherController.CurrentWeaponIndex - 1].Shoot(Input.GetKey(shootKey));
        }
    }
}