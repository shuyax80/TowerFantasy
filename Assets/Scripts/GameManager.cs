using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("XpCurveParams")]
    [SerializeField] private int _baseXP = 100; 
    [SerializeField] private float _multiplier = 1.2f; 
    [SerializeField] private float _exponent = 2.2f;   
    
    private long _playerXp = 0;
    
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        Instance = this;
    }
    private int GetXPForLevel(int level)
    {
        return Mathf.RoundToInt(_baseXP + (_multiplier * Mathf.Pow(level, _exponent)));
    }

    public void AddXP(long amount)
    {
        _playerXp += amount;
    }
}
