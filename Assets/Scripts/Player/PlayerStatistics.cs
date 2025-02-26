using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatistics : MonoBehaviour
{
    // ==============================
    // Serialized Fields
    // ==============================
    [SerializeField] private int maxHealth;
    [SerializeField] private TMP_Text scoreDisplayText;
    [SerializeField] private Slider healthSlider;

    // ==============================
    // Public Properties
    // ==============================
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;

            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    public int Score;

    // ==============================
    // Private Variables
    // ==============================
    private int currentHealth;

    // ==============================
    // Unity Methods
    // ==============================
    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    // ==============================
    // Public Methods
    // ==============================
    public void Damage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
    }

    public void Healing(int healingAmount)
    {
        CurrentHealth += healingAmount;
    }

    // ==============================
    // Private Methods
    // ==============================
    private void Death()
    {
        // Здесь можно добавить логику для смерти
        Destroy(gameObject);
    }

    private void InitializeUI()
    {
        if (scoreDisplayText != null)
        {
            scoreDisplayText.text = $"Score: {Score}";
        }
        if (healthSlider != null)
        {
            CurrentHealth = maxHealth;
            healthSlider.maxValue = maxHealth;
        }
    }

    private void UpdateUI()
    {
        if (scoreDisplayText != null)
        {
            scoreDisplayText.text = $"Score: {Score}";
        }
        if (healthSlider != null)
        {
            healthSlider.value = CurrentHealth;
        }
    }
}