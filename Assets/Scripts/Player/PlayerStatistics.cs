using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatistics : MonoBehaviour
{
    public int CurrentHealth
    {
        get => currentHealth;
        private set
        {
            currentHealth = value;

            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }
    public int Score;

    [SerializeField] private int maxHealth;
    [SerializeField] private TMP_Text scoreDisplayText;
    [SerializeField] private Slider healthSlider;

    private int currentHealth;

    void Start()
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

    private void Update()
    {
        UpdateUI();
    }

    public void Damage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
    }

    private void Death()
    {
        // Здесь можно добавить логику для смерти
        Destroy(gameObject);
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