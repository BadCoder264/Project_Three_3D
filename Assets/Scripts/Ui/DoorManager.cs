using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour, IInteractive
{
    [SerializeField] private GameObject selectLocationUi;
    [SerializeField] private InputListener inputListener;

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (selectLocationUi == null)
            return;

        selectLocationUi.SetActive(true);

        if (this.inputListener == null)
            return;

        this.inputListener.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ExitTheSelectLocation()
    {
        if (selectLocationUi == null)
            return;

        selectLocationUi.SetActive(false);

        if (this.inputListener == null)
            return;

        this.inputListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void ChooseLocation(string nameLocation)
    {
        if (string.IsNullOrEmpty(nameLocation))
            return;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nameLocation);
    }
}