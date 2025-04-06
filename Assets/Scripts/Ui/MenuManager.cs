using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private PlayerStatistics playerStatistics;
    [SerializeField] private UpgradeManager upgradeManager;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Continue()
    {
        LoadAll();
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        LoadAll();
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
            return;

        if (upgradeManager == null)
            return;

        playerStatistics.LoadUpgrade();
        upgradeManager.LoadUpgrade();
    }
}