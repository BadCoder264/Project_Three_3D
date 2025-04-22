using UnityEngine;

public class SaveOrLoad : MonoBehaviour
{
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private UpgradeManager upgradeManager;

    public void LoadPlayer()
    {
        if (playerStatistics == null)
            return;

        playerStatistics.IsMedicalUpgrade = PlayerPrefs.HasKey("HealthRecoveryPercentage");
        playerStatistics.IsTrainingUpgrade = PlayerPrefs.HasKey("RecoilReductionPercentage");
        playerStatistics.IsCraftingUpgrade = PlayerPrefs.HasKey("DamageIncreasePercentage");

        if (playerStatistics.IsMedicalUpgrade)
        {
            playerStatistics.HealthRecoveryPercentage = PlayerPrefs.GetInt("HealthRecoveryPercentage");
        }

        if (playerStatistics.IsTrainingUpgrade)
        {
            playerStatistics.RecoilReductionPercentage = PlayerPrefs.GetInt("RecoilReductionPercentage");
        }

        if (playerStatistics.IsCraftingUpgrade)
        {
            playerStatistics.DamageIncreasePercentage = PlayerPrefs.GetInt("DamageIncreasePercentage");
        }

        if (PlayerPrefs.HasKey("SavedScore"))
        {
            playerStatistics.Score = PlayerPrefs.GetInt("SavedScore");
        }
    }

    public void SavePlayer()
    {
        if (playerStatistics == null)
            return;

        PlayerPrefs.SetInt("SavedScore", playerStatistics.Score);
    }

    public void LoadUpgrade()
    {
        if (upgradeManager == null)
            return;

        for (int i = 0; i < upgradeManager.roomStatics.Count; i++)
        {
            if (PlayerPrefs.HasKey("RoomLevel_" + i))
            {
                upgradeManager.roomStatics[i].LevelRoom = PlayerPrefs.GetInt("RoomLevel_" + i);
            }

            if (PlayerPrefs.HasKey("RoomPrice_" + i))
            {
                upgradeManager.roomStatics[i].PriseLevelRoom = PlayerPrefs.GetInt("RoomPrice_" + i);
            }

            if (PlayerPrefs.HasKey("RoomOpen_" + i))
            {
                int roomOpenValue = PlayerPrefs.GetInt("RoomOpen_" + i);
            }

            upgradeManager.roomStatics[i].IsOpen = PlayerPrefs.HasKey("RoomOpen_" + i) ? false : true;
            upgradeManager.roomStatics[i].DoorRoom.SetActive(upgradeManager.roomStatics[i].IsOpen);

            if (upgradeManager.roomStatics[i].LevelRoom >= 5)
            {
                upgradeManager.roomStatics[i].Button.interactable = false;
            }
        }
    }

    public void SaveUpgrade(int indexRoom)
    {
        if (upgradeManager == null)
            return;

        PlayerPrefs.SetInt("RoomPrice_" + indexRoom, upgradeManager.roomStatics[indexRoom].PriseLevelRoom);
        PlayerPrefs.SetInt("RoomLevel_" + indexRoom, upgradeManager.roomStatics[indexRoom].LevelRoom);

        PlayerPrefs.SetInt("HealthRecoveryPercentage", playerStatistics.HealthRecoveryPercentage);
        PlayerPrefs.SetInt("RecoilReductionPercentage", playerStatistics.RecoilReductionPercentage);
        PlayerPrefs.SetInt("DamageIncreasePercentage", playerStatistics.DamageIncreasePercentage);

        PlayerPrefs.SetInt("RoomOpen_" + indexRoom, upgradeManager.roomStatics[indexRoom].IsOpen ? 1 : 0);
    }
}