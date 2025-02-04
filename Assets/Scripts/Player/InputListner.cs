using System.Collections.Generic;
using UnityEngine;

public class InputListner : MonoBehaviour
{
    [SerializeField] private CameraRotate cameraRotateController;
    [SerializeField] private PlayerMovement playerMovementController;
    [SerializeField] private PickUpWeapon weaponPicker;
    public List<GameObject> PlayerWeaponsList;
    public List<PlayerShoot> PlayerShootingList;
    public WeaponSwitcher weaponSwitcherController;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode interactiveKey = KeyCode.E;

    private void Start()
    {
        PlayerShootingList = new List<PlayerShoot>();
        if (weaponSwitcherController != null)
        {
            weaponSwitcherController.Initialize(PlayerWeaponsList);
        }
    }

    private void Update()
    {
        cameraRotateController?.RotateCamera();

        playerMovementController?.Move();
        playerMovementController?.Sprint(Input.GetKey(sprintKey));

        weaponPicker?.PickUp(Input.GetKey(pickUpKey));

        if (weaponSwitcherController != null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                weaponSwitcherController.SwitchWeapon(1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                weaponSwitcherController.SwitchWeapon(-1);
            }
        }

        if (PlayerShootingList.Count > 0 && weaponSwitcherController.CurrentWeaponIndex >= 0)
        {
            PlayerShootingList[weaponSwitcherController.CurrentWeaponIndex].Shoot(Input.GetKey(shootKey));
        }

        waveManager?.OnWaveStartButtonPressed(Input.GetKeyDown(interactiveKey));
    }
}
