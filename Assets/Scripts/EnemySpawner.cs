using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<GameObject> _spawnPoints;
    [SerializeField] private float spawnRate;
    [SerializeField] private GameObject meteorite;
    void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Respawn").ToList();
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            var selectedSpawn = _spawnPoints[Random.Range(0, _spawnPoints.Count)].transform;
            Instantiate(meteorite, selectedSpawn.position, Quaternion.identity);
        } 
    }
}
