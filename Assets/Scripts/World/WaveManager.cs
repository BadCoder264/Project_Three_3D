using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private int initialMaxEnemiesPerWave;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text waveDisplayText;
    [SerializeField] private TMP_Text startWavePromptText;
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
            EndCurrentWave();
        }

        UpdateUI();
    }

    public void StartNewWave()
    {
        if (!isWaveActive)
        {
            currentWaveIndex++;
            initialMaxEnemiesPerWave += 5;
            isWaveActive = true;

            float spawnInterval = Random.Range(0.2f, 0.4f);
            StartCoroutine(SpawnEnemiesCoroutine(spawnInterval));
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(float spawnInterval)
    {
        for (int i = 0; i < initialMaxEnemiesPerWave; i++)
        {
            if (enemyPrefabs.Count > 0 && spawnPoints.Count > 0)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Count);
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
                activeEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void EndCurrentWave()
    {
        isWaveActive = false;
    }

    private void UpdateUI()
    {
        UpdateWaveDisplay();
        UpdateEnemyCountSlider();
        UpdateStartWavePrompt();
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
            enemyCountSlider.maxValue = initialMaxEnemiesPerWave;
            enemyCountSlider.value = activeEnemies.Count;
        }
    }

    private void UpdateStartWavePrompt()
    {
        if (playerCamera != null && startWavePromptText != null)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            startWavePromptText.gameObject.SetActive(Physics.Raycast(ray, rayDistance, raycastLayerMask));
        }
    }
}