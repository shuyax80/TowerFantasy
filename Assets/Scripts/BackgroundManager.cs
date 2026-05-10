using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [Header("Fixed Start Segments")]
    public GameObject firstSegment;
    public GameObject secondSegment;

    [Header("Random Segments")]
    public GameObject[] randomSegments;

    [Header("References")]
    public Camera mainCamera;

    [Header("Settings")]
    public float segmentHeight = 20f;
    public int segmentsAhead = 4;
    public int segmentsBehind = 2;

    private readonly List<GameObject> _activeSegments = new();

    private float _nextSpawnY = 0f;
    private bool _initialSegmentsSpawned = false;

    void Start()
    {
        SpawnInitialSegments();

        for (int i = 0; i < segmentsAhead; i++)
        {
            SpawnRandomSegment();
        }
    }

    void Update()
    {
        float cameraY = mainCamera.transform.position.y;

        while (cameraY + segmentsAhead * segmentHeight > _nextSpawnY)
        {
            SpawnRandomSegment();
        }

        CleanupOldSegments(cameraY);
    }

    void SpawnInitialSegments()
    {
        if (_initialSegmentsSpawned)
            return;

        SpawnSpecificSegment(firstSegment);
        SpawnSpecificSegment(secondSegment);

        _initialSegmentsSpawned = true;
    }

    void SpawnRandomSegment()
    {
        GameObject prefab =
            randomSegments[Random.Range(0, randomSegments.Length)];

        SpawnSpecificSegment(prefab);
    }

    void SpawnSpecificSegment(GameObject prefab)
    {
        Vector3 position = new Vector3(0f, _nextSpawnY, 0f);

        GameObject segment =
            Instantiate(prefab, position, Quaternion.identity, transform);

        _activeSegments.Add(segment);

        _nextSpawnY += segmentHeight;
    }

    void CleanupOldSegments(float cameraY)
    {
        var minY = cameraY - segmentsBehind * segmentHeight;

        for (var i = _activeSegments.Count - 1; i >= 0; i--)
        {
            if (_activeSegments[i].transform.position.y + segmentHeight < minY)
            {
                Destroy(_activeSegments[i]);
                _activeSegments.RemoveAt(i);
            }
        }
    }
}