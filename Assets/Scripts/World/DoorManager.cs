using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour, IInteractive
{
    [SerializeField] private List<string> nameLocation;

    private int indexLocation;

    void Start()
    {
        indexLocation = Random.Range(0, nameLocation.Count);
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        SceneManager.LoadScene(nameLocation[indexLocation]);
    }
}