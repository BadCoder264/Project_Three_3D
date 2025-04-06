using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public void Pause(InputListener inputListener)
    {
        if (inputListener.pause)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void Continue(InputListener inputListener)
    {
        gameObject.SetActive(false);
        inputListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}