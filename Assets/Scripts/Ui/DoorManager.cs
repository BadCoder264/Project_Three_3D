using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour, IInteractive
{
    [SerializeField] private GameObject selectLocationUi;
    [SerializeField] private InputListener inputListener;

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (selectLocationUi == null)
        {
            Debug.LogError("Select Location UI is not assigned!", this);
            return;
        }

        selectLocationUi.SetActive(true);

        if (this.inputListener == null)
        {
            Debug.LogError("Input Listener is not assigned!", this);
            return;
        }

        this.inputListener.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void ExitTheSelectLocation()
    {
        if (selectLocationUi == null)
        {
            Debug.LogError("Select Location UI is not assigned!", this);
            return;
        }

        selectLocationUi.SetActive(false);

        if (this.inputListener == null)
        {
            Debug.LogError("Input Listener is not assigned!", this);
            return;
        }

        this.inputListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void ChooseLocation(string nameLocation)
    {
        if (string.IsNullOrEmpty(nameLocation))
        {
            Debug.LogError("Location name is null or empty!", this);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(nameLocation);
    }
}