using UnityEngine;

public interface IInteractive
{
    void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler);
}