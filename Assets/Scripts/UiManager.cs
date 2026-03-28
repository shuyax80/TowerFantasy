using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI levelText;
    private long _currentScore = 0;
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

    public void AddScore(long amount)
    {
        _currentScore += amount;
        
    }
}
