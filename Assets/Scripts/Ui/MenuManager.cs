using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private UpgradeManager upgradeManager;

    public void Continue()
    {
        LoadAll();
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        LoadAll();
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Activate(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void Deactivate(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    private void LoadAll()
    {
        if (playerStatistics == null)
        {
            Debug.LogError("PlayerStatistics not found!");
            return;
        }

        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager not found!");
            return;
        }

        playerStatistics.LoadUpgrade();
        upgradeManager.LoadUpgrade();
    }
}