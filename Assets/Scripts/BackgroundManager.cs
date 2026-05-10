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

    private readonly List<GameObject> activeSegments = new();

    private float nextSpawnY = 0f;
    private bool initialSegmentsSpawned = false;

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

        while (cameraY + segmentsAhead * segmentHeight > nextSpawnY)
        {
            SpawnRandomSegment();
        }

        CleanupOldSegments(cameraY);
    }

    void SpawnInitialSegments()
    {
        if (initialSegmentsSpawned)
            return;

        SpawnSpecificSegment(firstSegment);
        SpawnSpecificSegment(secondSegment);

        initialSegmentsSpawned = true;
    }

    void SpawnRandomSegment()
    {
        GameObject prefab =
            randomSegments[Random.Range(0, randomSegments.Length)];

        SpawnSpecificSegment(prefab);
    }

    void SpawnSpecificSegment(GameObject prefab)
    {
        Vector3 position = new Vector3(0f, nextSpawnY, 0f);

        GameObject segment =
            Instantiate(prefab, position, Quaternion.identity, transform);

        activeSegments.Add(segment);

        nextSpawnY += segmentHeight;
    }

    void CleanupOldSegments(float cameraY)
    {
        float minY = cameraY - segmentsBehind * segmentHeight;

        for (int i = activeSegments.Count - 1; i >= 0; i--)
        {
            if (activeSegments[i].transform.position.y + segmentHeight < minY)
            {
                Destroy(activeSegments[i]);
                activeSegments.RemoveAt(i);
            }
        }
    }
}