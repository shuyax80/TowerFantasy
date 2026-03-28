using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> _spawnPoints;
    private List<GameObject> _enemies;
    
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject meteorite;
    
    public static EnemySpawner Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _enemies = new List<GameObject>();
        _spawnPoints = GameObject.FindGameObjectsWithTag("Respawn").ToList();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            var selectedSpawn = _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform;
            _enemies.Add(Instantiate(meteorite, selectedSpawn.position, Quaternion.identity));
        } 
    }
    
    public GameObject GetClosestEnemy(Vector3 origin)
    {
        _enemies.RemoveAll(item => item.IsUnityNull());

        if (_enemies.Count == 0) return null;

        GameObject closest = null;
        var minDistance = Mathf.Infinity;

        foreach (var enemy in _enemies)
        {
            var distance = Vector3.Distance(origin, enemy.transform.position);
            
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }
}
