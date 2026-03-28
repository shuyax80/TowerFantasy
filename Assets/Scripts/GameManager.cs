using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("XpCurveParams")]
    [SerializeField] private int baseXp = 100; 
    [SerializeField] private float multiplier = 1.2f; 
    [SerializeField] private float exponent = 2.2f;   
    
    private long _playerXp = 0;
    private int _xpForLevel = 0;
    private int _playerLevel = 1;
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } 
        Instance = this;
        _xpForLevel = GetXpForLevel(_playerLevel);
          
    }

    private void Start()
    {
        UiManager.Instance.UpdateXpBar(_playerXp, _xpForLevel); 
    }

    private int GetXpForLevel(int level)
    {
        return Mathf.RoundToInt(baseXp + (multiplier * Mathf.Pow(level, exponent)));
    }

    public void AddXp(long amount)
    {
        _playerXp += amount;
        if (_playerXp >= _xpForLevel)
        {
            _playerXp = 0;
            _playerLevel++;
            _xpForLevel = GetXpForLevel(_playerLevel);
            Player.Instance.IncreaseLevel();
            UiManager.Instance.SetLevel(_playerLevel);
            if (_playerLevel % 2 == 0)
            {
                EnemySpawner.Instance.UpdateSpawnRate();
            }

            if (_playerLevel % 5 == 0)
            {
                EnemySpawner.Instance.UpdateMultiplier();
            }
        }
        UiManager.Instance.UpdateXpBar(_playerXp, _xpForLevel); 
    }
}
