using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image xpBar;
    [SerializeField] private Image healthBar;
    private long _level = 1;
    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
        SetLevel(1);
    }

    public void SetLevel(long level)
    {
        _level = level;
        levelText.text = $"Level: {_level}";
    }

    public void UpdateXpBar(long playerXp, long xpForLevel)
    {
        xpBar.fillAmount = (float) playerXp / xpForLevel;
    }
    
    public void UpdateHealthBar(long playerHealth, long maxHealth)
    {
        healthBar.fillAmount = (float) playerHealth / maxHealth;
    }
}
