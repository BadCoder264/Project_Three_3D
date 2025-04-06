using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public int DamageIncreasePercentage;
    public bool IsMedicalUpgrade;
    public bool IsTrainingUpgrade;
    public bool IsCraftingUpgrade;

    [SerializeField] private int maxHealth;
    [SerializeField] private TMP_Text scoreDisplayText;
    [SerializeField] private Slider healthSlider;

    private int currentHealth;
    private float healthRegenTimer = 0f;

    private void Start()
    {
        LoadUpgrade();
        InitializeUI();
    }

    private void Update()
    {
        UpdateUI();
        HealthRegeneration();
    }

    public void LoadUpgrade()
    {
        IsMedicalUpgrade = PlayerPrefs.HasKey("HealthRecoveryPercentage");
        IsTrainingUpgrade = PlayerPrefs.HasKey("RecoilReductionPercentage");
        IsCraftingUpgrade = PlayerPrefs.HasKey("DamageIncreasePercentage");

        if (IsMedicalUpgrade)
        {
            HealthRecoveryPercentage = PlayerPrefs.GetInt("HealthRecoveryPercentage");
        }

        if (IsTrainingUpgrade)
        {
            RecoilReductionPercentage = PlayerPrefs.GetInt("RecoilReductionPercentage");
        }

        if (IsCraftingUpgrade)
        {
            DamageIncreasePercentage = PlayerPrefs.GetInt("DamageIncreasePercentage");
        }

        if (PlayerPrefs.HasKey("SavedScore"))
        {
            Score = PlayerPrefs.GetInt("SavedScore");
        }
    }

    public void Damage(int damageAmount)
    {
        if (damageAmount > 0)
        {
            CurrentHealth -= damageAmount;
        }
    }

    public void Healing(int healingAmount)
    {
        if (healingAmount > 0)
        {
            CurrentHealth += healingAmount;
        }
    }

    private void Death()
    {
        PlayerPrefs.SetInt("SavedScore", Score);
        SceneManager.LoadScene("MainMenu");
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
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = CurrentHealth;
        }
    }

    private void HealthRegeneration()
    {
        if (IsMedicalUpgrade)
        {
            healthRegenTimer += Time.deltaTime;

            if (healthRegenTimer >= 12f)
            {
                int healthIncrease = Mathf.FloorToInt(maxHealth * (HealthRecoveryPercentage / 100f));
                CurrentHealth += healthIncrease;
                healthRegenTimer = 0f;
            }
        }
    }
}