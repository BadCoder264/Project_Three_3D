using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int CurrentWeaponIndex { get; set; }

    [SerializeField] private InputListener inputListener;

    public void Initialize(List<GameObject> weaponList)
    {
        if (inputListener == null)
            return;

        inputListener.PlayerWeaponsList = weaponList ?? new List<GameObject>();

        if (weaponList == null || weaponList.Count == 0)
            return;

        CurrentWeaponIndex = 0;
        UpdateWeaponVisibility();
    }

    public void SwitchWeapon(int direction)
    {
        if (inputListener?.PlayerWeaponsList == null)
            return;

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
            return;

        for (int i = 0; i < inputListener.PlayerWeaponsList.Count; i++)
        {
            inputListener.PlayerWeaponsList[i].SetActive(i == CurrentWeaponIndex);
        }
    }
}