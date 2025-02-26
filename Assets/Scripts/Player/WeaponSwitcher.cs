using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [SerializeField] private InputListener inputListener;

    // ==============================
    // Public Variables
    // ==============================
    public int CurrentWeaponIndex;

    // ==============================
    // Public Methods
    // ==============================
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
            // Wrap around weapon index
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

    // ==============================
    // Private Methods
    // ==============================
    private void UpdateWeaponVisibility()
    {
        for (int i = 0; i < inputListener.PlayerWeaponsList.Count; i++)
        {
            // Set active weapon visibility
            inputListener.PlayerWeaponsList[i].SetActive(i == CurrentWeaponIndex);
        }
    }
}