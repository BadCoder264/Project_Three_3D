using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour, IInteractive
{
    [SerializeField] private int currentWaveMaxEnemies;
    [SerializeField] private int overallMaxEnemies;
    [SerializeField] private float minSpawnIntervalEnemies;
    [SerializeField] private float maxSpawnIntervalEnemies;
    [SerializeField] private float returnTimer;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private TMP_Text waveDisplayText;
    [SerializeField] private Slider enemyCountSlider;

    private int currentWaveIndex;
    private int IncreaseEnemyCount = 5;
    private bool isWaveActive;
    private bool isReturning = false;
    private List<GameObject> activeEnemies = new List<GameObject>();

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

        if (isReturning)
        {
            returnTimer -= Time.deltaTime;

            if (returnTimer <= 0f)
            {
                SceneManager.LoadScene(0);
                return;
            }
        }

        UpdateUI();
    }

    public void Interactive(PlayerStatistics playerStatistics, InputListener inputListener, PlayerShooting playerShoot, Transform weaponHandler)
    {
        if (!isWaveActive && !isReturning)
        {
            if (currentWaveIndex > 34)
            {
                isReturning = true;
            }
            else
            {
                currentWaveIndex++;
                currentWaveMaxEnemies = Mathf.Min(currentWaveMaxEnemies + IncreaseEnemyCount, overallMaxEnemies);
                isWaveActive = true;

                StartCoroutine(SpawnEnemiesCoroutine(Random.Range(minSpawnIntervalEnemies, maxSpawnIntervalEnemies)));
            }
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
            return;

        int spawnIndex = Random.Range(0, spawnPoints.Count);
        int enemyTypeIndex = Random.Range(0, maxEnemyTypeIndex + 1);

        GameObject enemy = Instantiate(enemyPrefabs[enemyTypeIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        activeEnemies.Add(enemy);
    }

    private int GetMaxEnemyTypeIndex(int waveIndex)
    {
        if (waveIndex < 6)
            return 0;

        if (waveIndex < 12)
            return 1;

        if (waveIndex < 18)
            return 2;

        if (waveIndex < 24)
            return 3;

        return 0;
    }

    private void UpdateUI()
    {
        if (waveDisplayText != null)
        {
            if (isReturning)
            {
                waveDisplayText.text = $"Time until return: {returnTimer.ToString("F2")}";
            }
            else
            {
                waveDisplayText.text = $"Wave: {currentWaveIndex}";
            }
        }

        if (enemyCountSlider != null)
        {
            enemyCountSlider.maxValue = currentWaveMaxEnemies;
            enemyCountSlider.value = activeEnemies.Count;
        }
    }
}