using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int CurrentWeaponIndex;

    [SerializeField] private InputListener inputListener;

    public void Initialize(List<GameObject> weaponList)
    {
        inputListener.PlayerWeaponsList = weaponList;
        CurrentWeaponIndex = 0;
        UpdateWeaponVisibility();
    }

    public void SwitchWeapon(int direction)
    {
        CurrentWeaponIndex += direction;

        if (inputListener != null)
        {
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
    }

    private void UpdateWeaponVisibility()
    {
        for (int i = 0; i < inputListener.PlayerWeaponsList.Count; i++)
        {
            inputListener.PlayerWeaponsList[i].SetActive(i == CurrentWeaponIndex);
        }
    }
}