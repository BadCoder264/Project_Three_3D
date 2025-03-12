using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatistics : MonoBehaviour
{
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
    private int currentHealth;

    [SerializeField] private int maxHealth;
    [SerializeField] private TMP_Text scoreDisplayText;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        InitializeUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void Damage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
    }

    public void Healing(int healingAmount)
    {
        CurrentHealth += healingAmount;
    }

    private void Death()
    {
        // Add logic for enemy death here
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