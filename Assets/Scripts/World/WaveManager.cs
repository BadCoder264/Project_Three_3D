using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour, IInteractive
{
    [SerializeField] private int currentWaveMaxEnemies;
    [SerializeField] private int overallMaxEnemies;
    [SerializeField] private float minSpawnIntervalEnemies;
    [SerializeField] private float maxSpawnIntervalEnemies;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private TMP_Text waveDisplayText;
    [SerializeField] private Slider enemyCountSlider;

    private int currentWaveIndex;
    private bool isWaveActive;
    private List<GameObject> activeEnemies = new List<GameObject>();

    private int IncreaseEnemyCount = 5;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (activeEnemies.Count == 0 && isWaveActive)
        {
            isWaveActive = false;
        }

        UpdateUI();
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (!isWaveActive)
        {
            currentWaveIndex++;
            currentWaveMaxEnemies = Mathf.Min(currentWaveMaxEnemies + IncreaseEnemyCount, overallMaxEnemies);
            isWaveActive = true;

            StartCoroutine(SpawnEnemiesCoroutine(Random.Range(minSpawnIntervalEnemies, maxSpawnIntervalEnemies)));
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(float spawnInterval)
    {
        int maxEnemyTypeIndex = GetMaxEnemyTypeIndex(currentWaveIndex);

        for (int i = 0; i < currentWaveMaxEnemies; i++)
        {
            if (spawnPoints.Count > 0)
            {
                SpawnEnemy(maxEnemyTypeIndex);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy(int maxEnemyTypeIndex)
    {
        if (enemyPrefabs.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Enemy prefabs or spawn points are not assigned!", this);
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Count);
        int enemyTypeIndex = Random.Range(0, maxEnemyTypeIndex + 1);

        GameObject enemy = Instantiate(enemyPrefabs[enemyTypeIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        activeEnemies.Add(enemy);
    }

    private int GetMaxEnemyTypeIndex(int waveIndex)
    {
        if (waveIndex < 15) return 0;
        if (waveIndex < 30) return 1;
        if (waveIndex < 45) return 2;
        return 3;
    }

    private void UpdateUI()
    {
        if (waveDisplayText != null)
        {
            waveDisplayText.text = $"Wave: {currentWaveIndex}";
        }
        else
        {
            Debug.LogWarning("Wave Display Text is not assigned!", this);
        }

        if (enemyCountSlider != null)
        {
            enemyCountSlider.maxValue = currentWaveMaxEnemies;
            enemyCountSlider.value = activeEnemies.Count;
        }
        else
        {
            Debug.LogWarning("Enemy Count Slider is not assigned!", this);
        }
    }
}