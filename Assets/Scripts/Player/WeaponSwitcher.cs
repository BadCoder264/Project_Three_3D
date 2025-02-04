using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private InputListner inputListner;

    public int CurrentWeaponIndex { get; set; }

    public void Initialize(List<GameObject> weaponList)
    {
        inputListner.PlayerWeaponsList = weaponList;
        CurrentWeaponIndex = 0;
        UpdateWeaponVisibility();
    }

    public void SwitchWeapon(int direction)
    {
        CurrentWeaponIndex += direction;

        if (inputListner != null)
        {
            if (CurrentWeaponIndex >= inputListner.PlayerWeaponsList.Count)
            {
                CurrentWeaponIndex = 0;
            }
            else if (CurrentWeaponIndex < 0)
            {
                CurrentWeaponIndex = inputListner.PlayerWeaponsList.Count - 1;
            }

            UpdateWeaponVisibility();
        }
    }

    private void UpdateWeaponVisibility()
    {
        for (int i = 0; i < inputListner.PlayerWeaponsList.Count; i++)
        {
            inputListner.PlayerWeaponsList[i].SetActive(i == CurrentWeaponIndex);
        }
    }
}