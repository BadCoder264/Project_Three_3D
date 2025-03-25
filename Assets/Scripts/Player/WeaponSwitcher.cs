using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int CurrentWeaponIndex { get; set; }

    [SerializeField] private InputListener inputListener;

    public void Initialize(List<GameObject> weaponList)
    {
        if (inputListener == null)
        {
            Debug.LogError("Input Listener is not assigned!", this);
            return;
        }

        inputListener.PlayerWeaponsList = weaponList ?? new List<GameObject>();

        if (weaponList == null || weaponList.Count == 0)
        {
            Debug.LogError("Weapon list is null or empty!", this);
            return;
        }

        CurrentWeaponIndex = 0;
        UpdateWeaponVisibility();
    }

    public void SwitchWeapon(int direction)
    {
        if (inputListener?.PlayerWeaponsList == null)
        {
            Debug.LogError("Input Listener or Player Weapons List is not assigned!", this);
            return;
        }

        CurrentWeaponIndex += direction;

        if (CurrentWeaponIndex >= inputListener.PlayerWeaponsList.Count)
        {
            CurrentWeaponIndex = 0;
        }
        else if (CurrentWeaponIndex < 0)
        {
            CurrentWeaponIndex = inputListener.PlayerWeaponsList.Count - 1;
        }

        UpdateWeaponVisibility();
    }

    private void UpdateWeaponVisibility()
    {
        if (inputListener?.PlayerWeaponsList == null)
        {
            Debug.LogError("Input Listener or Player Weapons List is not assigned!", this);
            return;
        }

        for (int i = 0; i < inputListener.PlayerWeaponsList.Count; i++)
        {
            inputListener.PlayerWeaponsList[i].SetActive(i == CurrentWeaponIndex);
        }
    }
}