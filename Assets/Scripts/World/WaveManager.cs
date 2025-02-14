using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int currentWaveMaxEnemies;
    [SerializeField] private int overallMaxEnemies;
    [SerializeField] private float minSpawnIntervalEnemies;
    [SerializeField] private float maxSpawnIntervalEnemies;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text waveDisplayText;
    [SerializeField] private Slider enemyCountSlider;

    private int currentWaveIndex;
    private bool isWaveActive;
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

        UpdateUI();
    }

    public void StartNewWave()
    {
        if (!isWaveActive)
        {
            currentWaveIndex++;
            if (currentWaveMaxEnemies < overallMaxEnemies)
            {
                currentWaveMaxEnemies += 5;
            }
            else
            {
                currentWaveMaxEnemies = overallMaxEnemies;
            }
            isWaveActive = true;

            float spawnInterval = Random.Range(minSpawnIntervalEnemies, maxSpawnIntervalEnemies);
            StartCoroutine(SpawnEnemiesCoroutine(spawnInterval));
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(float spawnInterval)
    {
        for (int i = 0; i < currentWaveMaxEnemies; i++)
        {
            if (enemyPrefabs != null && spawnPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                GameObject enemy = Instantiate(enemyPrefabs, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
                activeEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
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