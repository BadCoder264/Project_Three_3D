using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatistics : MonoBehaviour
{
    public int Score;
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }
    public int HealthRecoveryPercentage;
    public int RecoilReductionPercentage;
    public bool IsMedicalUpgrade;
    public bool IsTrainingUpgrade;

    [SerializeField] private int maxHealth;
    [SerializeField] private TMP_Text scoreDisplayText;
    [SerializeField] private Slider healthSlider;

    private int currentHealth;

    private void Start()
    {
        LoadUpgrade();
        InitializeUI();
        StartCoroutine(HealthRegeneration());
    }

    private void Update()
    {
        UpdateUI();
    }

    public void LoadUpgrade()
    {
        IsMedicalUpgrade = PlayerPrefs.HasKey("HealthRecoveryPercentage");
        IsTrainingUpgrade = PlayerPrefs.HasKey("RecoilReductionPercentage");

        if (PlayerPrefs.HasKey("HealthRecoveryPercentage"))
        {
            HealthRecoveryPercentage = PlayerPrefs.GetInt("HealthRecoveryPercentage");
        }

        if (PlayerPrefs.HasKey("RecoilReductionPercentage"))
        {
            RecoilReductionPercentage = PlayerPrefs.GetInt("RecoilReductionPercentage");
        }
    }

    public void Damage(int damageAmount)
    {
        if (damageAmount > 0)
        {
            CurrentHealth -= damageAmount;
        }
        else
        {
            Debug.LogError("Damage amount cannot be negative!", this);
        }
    }

    public void Healing(int healingAmount)
    {
        if (healingAmount > 0)
        {
            CurrentHealth += healingAmount;
        }
        else
        {
            Debug.LogError("Healing amount cannot be negative!", this);
        }
    }

    private void Death()
    {
        // Логика смерти игрока
        Destroy(gameObject);
    }

    private void InitializeUI()
    {
        UpdateScoreDisplay();
        CurrentHealth = maxHealth;
        UpdateHealthSlider();
    }

    private void UpdateUI()
    {
        UpdateScoreDisplay();
        UpdateHealthSlider();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreDisplayText != null)
        {
            scoreDisplayText.text = $"Score: {Score}";
        }
        else
        {
            Debug.LogError("Score Display Text is not assigned!", this);
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = CurrentHealth;
        }
        else
        {
            Debug.LogError("Health Slider is not assigned!", this);
        }
    }

    private IEnumerator HealthRegeneration()
    {
        while (true)
        {
            if (IsMedicalUpgrade)
            {
                int healthIncrease = Mathf.FloorToInt(maxHealth * (HealthRecoveryPercentage / 100f));
                CurrentHealth += healthIncrease;
            }
            yield return new WaitForSeconds(12);
        }
    }
}