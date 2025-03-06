using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void Continue()
    {

    }

    public void NewGame()
    {

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
}