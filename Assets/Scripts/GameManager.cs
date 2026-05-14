using System;
using UnityEngine;

public class GameManager : MonoBehaviour, ISaveable
{
    [Serializable]
    private class GameManagerSaveData
    {
        public long playerXp;
        public int playerLevel;
    }

    [Header("XpCurveParams")]
    [SerializeField] private int baseXp = 100; 
    [SerializeField] private float multiplier = 1.2f; 
    [SerializeField] private float exponent = 2.2f;   
    
    private long _playerXp = 0;
    private int _xpForLevel = 0;
    private int _playerLevel = 1;
    public static GameManager Instance { get; private set; }
    public string SaveId => "game_manager";
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
        UpdateUi();
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
        }
        UpdateUi();
    }

    public string Save()
    {
        var data = new GameManagerSaveData
        {
            playerXp = _playerXp,
            playerLevel = _playerLevel
        };

        return JsonUtility.ToJson(data);
    }

    public void Load(string json)
    {
        var data = JsonUtility.FromJson<GameManagerSaveData>(json);
        if (data == null)
        {
            return;
        }

        _playerXp = Math.Max(0, data.playerXp);
        _playerLevel = Mathf.Max(1, data.playerLevel);
        _xpForLevel = GetXpForLevel(_playerLevel);
        UpdateUi();
    }

    private void UpdateUi()
    {
        UiManager.Instance.SetLevel(_playerLevel);
        UiManager.Instance.UpdateXpBar(_playerXp, _xpForLevel);
    }
}
