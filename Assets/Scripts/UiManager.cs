using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    private long _currentScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void AddScore(long amount)
    {
        _currentScore += amount;
        scoreText.text = $"{_currentScore}";
    }
}
