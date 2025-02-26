using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour, IInteractive
{
    // ==============================
    // Serialized Fields
    // ==============================
    [Header("Wave Settings")]
    [SerializeField] private int currentWaveMaxEnemies;
    [SerializeField] private int overallMaxEnemies;
    [SerializeField] private float minSpawnIntervalEnemies;
    [SerializeField] private float maxSpawnIntervalEnemies;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text waveDisplayText;
    [SerializeField] private Slider enemyCountSlider;

    // ==============================
    // Private Variables
    // ==============================
    private int currentWaveIndex;
    private bool isWaveActive;
    private List<GameObject> activeEnemies = new List<GameObject>();

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        CleanUpActiveEnemies();
        CheckWaveStatus();
        UpdateUI();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (!isWaveActive)
        {
            StartNewWave();
        }
    }

    // ==============================
    // Private Methods
    // ==============================
    private void CleanUpActiveEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    private void CheckWaveStatus()
    {
        if (activeEnemies.Count == 0 && isWaveActive)
        {
            isWaveActive = false;
        }
    }

    private void StartNewWave()
    {
        currentWaveIndex++;
        UpdateCurrentWaveMaxEnemies();
        isWaveActive = true;

        float spawnInterval = Random.Range(minSpawnIntervalEnemies, maxSpawnIntervalEnemies);
        StartCoroutine(SpawnEnemiesCoroutine(spawnInterval));
    }

    private void UpdateCurrentWaveMaxEnemies()
    {
        if (currentWaveMaxEnemies < overallMaxEnemies)
        {
            currentWaveMaxEnemies += 5;
        }
        else
        {
            currentWaveMaxEnemies = overallMaxEnemies;
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
        UpdateWaveDisplay();
        UpdateEnemyCountSlider();
    }

    private void UpdateWaveDisplay()
    {
        if (waveDisplayText != null)
        {
            waveDisplayText.text = $"Wave: {currentWaveIndex}";
        }
    }

    private void UpdateEnemyCountSlider()
    {
        if (enemyCountSlider != null)
        {
            enemyCountSlider.maxValue = currentWaveMaxEnemies;
            enemyCountSlider.value = activeEnemies.Count;
        }
    }
}