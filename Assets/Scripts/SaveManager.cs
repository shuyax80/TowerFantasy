using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private string saveFileName = "save.json";
    [SerializeField] private bool loadOnStart = true;

    public static SaveManager Instance { get; private set; }

    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (loadOnStart)
        {
            LoadGame();
        }
    }

    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        var saveData = new SaveData();
        var saveables = FindSaveables();

        foreach (var saveable in saveables)
        {
            if (string.IsNullOrWhiteSpace(saveable.SaveId))
            {
                Debug.LogWarning("Saveable object skipped because SaveId is empty.");
                continue;
            }

            if (saveData.entries.Any(entry => entry.id == saveable.SaveId))
            {
                Debug.LogWarning($"Duplicate SaveId skipped: {saveable.SaveId}");
                continue;
            }

            saveData.entries.Add(new SaveEntry
            {
                id = saveable.SaveId,
                json = saveable.Save()
            });
        }

        var json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SavePath, json);
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log($"No save file found at {SavePath}");
            return;
        }

        var json = File.ReadAllText(SavePath);
        var saveData = JsonUtility.FromJson<SaveData>(json);

        if (saveData?.entries == null)
        {
            Debug.LogWarning($"Save file is invalid: {SavePath}");
            return;
        }

        foreach (var saveable in FindSaveables())
        {
            var entry = saveData.entries.FirstOrDefault(saveEntry => saveEntry.id == saveable.SaveId);
            if (entry == null)
            {
                continue;
            }

            saveable.Load(entry.json);
        }
    }

    public bool HasSave()
    {
        return File.Exists(SavePath);
    }

    [ContextMenu("Delete Save")]
    public void DeleteSave()
    {
        if (!File.Exists(SavePath))
        {
            return;
        }

        File.Delete(SavePath);
        Debug.Log($"Save deleted: {SavePath}");
    }

    private static ISaveable[] FindSaveables()
    {
        return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToArray();
    }
}
