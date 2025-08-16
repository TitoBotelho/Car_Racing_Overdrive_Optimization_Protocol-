using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static MainManager Instance { get; private set; }

    // NEW: Selected car id and available car entries
    [System.Serializable]
    public class CarEntry {
        public int id;            // e.g., 1 = Janice, 2 = Lilly, 3 = Scarlett
        public GameObject prefab; // The prefab to spawn in game scene
    }

    [Header("Car Selection")]
    [SerializeField] public List<CarEntry> availableCars = new List<CarEntry>();
    public int SelectedCarId { get; private set; } = 2; // default to Lilly = 2

    [Header("Scenes")]
    [SerializeField] private string gameSceneName = "GameScene"; // Scene name to load from Start button

    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load persistent selection (replaces LoadColor)
        LoadSelection();
    }

    [System.Serializable]
    class SaveData
    {
        public int SelectedCarId;
    }

    // NEW: Select a car by id (hook this to UI buttons)
    public void SelectCar(int carId)
    {
        SelectedCarId = carId;
        SaveSelection();
    }

    // NEW: Return the prefab for the selected car (or null if not found)
    public GameObject GetSelectedCarPrefab()
    {
        for (int i = 0; i < availableCars.Count; i++)
        {
            if (availableCars[i] != null && availableCars[i].id == SelectedCarId)
            {
                return availableCars[i].prefab;
            }
        }
        return null;
    }

    // Optional helper: Try to spawn at a spawn point
    public GameObject TrySpawnSelectedCar(Transform spawnPoint)
    {
        var prefab = GetSelectedCarPrefab();
        if (prefab == null) return null;
        return Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    // NEW: Scene loading helpers for Start button
    public void LoadGameScene()
    {
        // Load by scene name set in the inspector
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game scene name is empty. Please set 'gameSceneName' in MainManager.");
        }
    }

    public void LoadGameSceneByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    // NEW: Save/Load selection
    public void SaveSelection()
    {
        SaveData data = new SaveData();
        data.SelectedCarId = SelectedCarId;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadSelection()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // If loaded id is non-zero, use it; otherwise fallback
            if (data.SelectedCarId != 0)
            {
                SelectedCarId = data.SelectedCarId;
            }
            else if (availableCars != null && availableCars.Count > 0)
            {
                SelectedCarId = availableCars[0].id; // fallback to first
            }
        }
        else
        {
            // No file yet: fallback to first available car if set
            if (availableCars != null && availableCars.Count > 0)
            {
                SelectedCarId = availableCars[0].id;
            }
        }
    }

}