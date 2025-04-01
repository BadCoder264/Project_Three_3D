using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour, IInteractive
{
    [SerializeField] private GameObject upgradeUi;
    [SerializeField] private TMP_Text descriptionUpgradeUi;
    [SerializeField] private InputListener inputListener;
    [SerializeField] private List<RoomStatics> roomStatics;

    public void LoadUpgrade()
    {
        for (int i = 0; i < roomStatics.Count; i++)
        {
            if (PlayerPrefs.HasKey("RoomLevel_" + i))
            {
                roomStatics[i].LevelRoom = PlayerPrefs.GetInt("RoomLevel_" + i);
            }

            if (PlayerPrefs.HasKey("RoomOpen_" + i))
            {
                int roomOpenValue = PlayerPrefs.GetInt("RoomOpen_" + i);
            }

            roomStatics[i].IsOpen = PlayerPrefs.HasKey("RoomOpen_" + i) ? false : true;
            roomStatics[i].DoorRoom.SetActive(roomStatics[i].IsOpen);
        }
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (upgradeUi == null || this.inputListener == null)
        {
            Debug.LogError("Upgrade UI or Input Listener is not assigned!", this);
            return;
        }

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
        {
            Debug.LogError("Room Statics list is not assigned or index is out of range!", this);
            return;
        }

        if (roomStatics[indexRoom].PriseLevelRoom <= playerStatistics.Score && roomStatics[indexRoom].LevelRoom < roomStatics.Count)
        {
            roomStatics[indexRoom].LevelRoom++;
            PlayerPrefs.SetInt("RoomLevel_" + indexRoom, roomStatics[indexRoom].LevelRoom);

            if (roomStatics[indexRoom].IsMedicalUpgrade)
            {
                playerStatistics.IsMedicalUpgrade = true;
                playerStatistics.HealthRecoveryPercentage += 5;
                PlayerPrefs.SetInt("HealthRecoveryPercentage", playerStatistics.HealthRecoveryPercentage);
            }
            else if (roomStatics[indexRoom].IsTrainingUpgrade)
            {
                playerStatistics.IsTrainingUpgrade = true;
                playerStatistics.RecoilReductionPercentage += 15;
                PlayerPrefs.SetInt("RecoilReductionPercentage", playerStatistics.RecoilReductionPercentage);
            }
            else if (roomStatics[indexRoom].IsCraftingUpgrade)
            {
                playerStatistics.IsCraftingUpgrade = true;
                playerStatistics.DamageIncreasePercentage += 5;
                PlayerPrefs.SetInt("DamageIncreasePercentage", playerStatistics.DamageIncreasePercentage);
            }

            roomStatics[indexRoom].IsOpen = true;
            roomStatics[indexRoom].DoorRoom.SetActive(!roomStatics[indexRoom].IsOpen);
            PlayerPrefs.SetInt("RoomOpen_" + indexRoom, roomStatics[indexRoom].IsOpen ? 1 : 0);
        }
    }

    public void ExitTheUpgrade()
    {
        if (upgradeUi == null || inputListener == null)
        {
            Debug.LogError("Upgrade UI or Input Listener is not assigned!", this);
            return;
        }

        upgradeUi.SetActive(false);
        this.inputListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void MouseEnterEvent(int indexRoom)
    {
        if (roomStatics == null || indexRoom < 0 || indexRoom >= roomStatics.Count || descriptionUpgradeUi == null)
        {
            Debug.LogError("Room Statics list is not assigned, index is out of range, or Description UI is not assigned!", this);
            return;
        }

        descriptionUpgradeUi.text = roomStatics[indexRoom].DescriptionRoom + "\n Current level: " + roomStatics[indexRoom].LevelRoom + "\n You need: " + roomStatics[indexRoom].PriseLevelRoom;
    }

    public void MouseExitEvent()
    {
        if (descriptionUpgradeUi != null)
        {
            descriptionUpgradeUi.text = "";
        }
        else
        {
            Debug.LogError("Description Upgrade UI TextMeshPro component is not assigned!", this);
        }
    }
}