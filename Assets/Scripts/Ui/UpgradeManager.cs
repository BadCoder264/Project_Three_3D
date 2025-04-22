using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IInteractive
{
    public List<RoomStatics> roomStatics;

    [SerializeField] private GameObject upgradeUi;
    [SerializeField] private TMP_Text descriptionUpgradeUi;
    [SerializeField] private InputListener inputListener;
    [SerializeField] private SaveOrLoad saveOrLoad;

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (upgradeUi == null || this.inputListener == null)
            return;

        upgradeUi.SetActive(true);
        this.inputListener.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void UpgradeRoom(int indexRoom)
    {
        PlayerStatistics playerStatistics = FindObjectOfType<PlayerStatistics>();

        if (roomStatics == null || indexRoom < 0 || indexRoom >= roomStatics.Count)
            return;

        if (roomStatics[indexRoom].PriseLevelRoom <= playerStatistics.Score && roomStatics[indexRoom].LevelRoom < 5)
        {
            playerStatistics.Score -= roomStatics[indexRoom].PriseLevelRoom;
            roomStatics[indexRoom].PriseLevelRoom += Mathf.RoundToInt(roomStatics[indexRoom].PriseLevelRoom * 0.12f);
            roomStatics[indexRoom].LevelRoom++;
            if (roomStatics[indexRoom].IsMedicalUpgrade)
            {
                playerStatistics.IsMedicalUpgrade = true;
                playerStatistics.HealthRecoveryPercentage += 5;
            }
            else if (roomStatics[indexRoom].IsTrainingUpgrade)
            {
                playerStatistics.IsTrainingUpgrade = true;
                playerStatistics.RecoilReductionPercentage += 15;
            }
            else if (roomStatics[indexRoom].IsCraftingUpgrade)
            {
                playerStatistics.IsCraftingUpgrade = true;
                playerStatistics.DamageIncreasePercentage += 5;
            }

            roomStatics[indexRoom].IsOpen = true;
            roomStatics[indexRoom].DoorRoom.SetActive(!roomStatics[indexRoom].IsOpen);

            if (saveOrLoad == null)
                return;

            saveOrLoad.SavePlayer();
            saveOrLoad.SaveUpgrade(indexRoom);

            MouseEnterEvent(indexRoom);
        }
        else if (roomStatics[indexRoom].LevelRoom >= 5)
        {
            roomStatics[indexRoom].Button.interactable = false;
        }
    }

    public void ExitTheUpgrade()
    {
        if (upgradeUi == null || inputListener == null)
            return;

        upgradeUi.SetActive(false);
        this.inputListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void MouseEnterEvent(int indexRoom)
    {
        if (roomStatics == null || indexRoom < 0 || indexRoom >= roomStatics.Count || descriptionUpgradeUi == null)
            return;

        descriptionUpgradeUi.text = roomStatics[indexRoom].DescriptionRoom + "\n Current level: " + roomStatics[indexRoom].LevelRoom + "\n You need: " + roomStatics[indexRoom].PriseLevelRoom;
    }

    public void MouseExitEvent()
    {
        if (descriptionUpgradeUi != null)
        {
            descriptionUpgradeUi.text = "";
        }
    }
}