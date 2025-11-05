using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadManager : MonoBehaviour
{
    private static SaveAndLoadManager _instance;
    public static SaveAndLoadManager Instance { get { return _instance; } }

    private string savePath;
    private PlayerData currentPlayerData;

    private void Awake()
    {
        // Singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Set save path
            savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            Debug.Log("Save path: " + savePath);
        }
    }

    private void OnEnable()
    {
        // Subscribe to scene loaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from scene loaded event
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a scene is loaded
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Auto-save when entering certain levels (you can modify this condition)
        if (ShouldAutoSave(scene.name))
        {
            AutoSave();
        }
    }

    // Define which levels should trigger auto-save
    private bool ShouldAutoSave(string sceneName)
    {
        // Add the names of levels where you want to auto-save
        string[] autoSaveLevels = { "Tutorial", "Day1House", "Day2House", "Day3House", "Day4House", "Day5House", "Day6House" };

        foreach (string level in autoSaveLevels)
        {
            if (sceneName == level)
            {
                return true;
            }
        }
        return false;
    }

    // Auto-save method
    public void AutoSave()
    {
        if (currentPlayerData != null)
        {
            SaveGame();
            Debug.Log("Game auto-saved!");
        }
    }

    // Save game data to JSON file
    public void SaveGame()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(currentPlayerData, true);
            File.WriteAllText(savePath, jsonData);
            Debug.Log("Game saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving game: " + e.Message);
        }
    }

    // Load game data from JSON file
    public void LoadGame()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string jsonData = File.ReadAllText(savePath);
                currentPlayerData = JsonUtility.FromJson<PlayerData>(jsonData);
                Debug.Log("Game loaded successfully!");

                if(currentPlayerData.CurrentDay == 0)
                {
                    SceneManager.LoadScene("Tutorial");
                }
                if (currentPlayerData.CurrentDay == 1)
                {
                    SceneManager.LoadScene("Day1House");
                }
                if (currentPlayerData.CurrentDay == 2)
                {
                    SceneManager.LoadScene("Day2House");
                }
                if (currentPlayerData.CurrentDay == 3)
                {
                    SceneManager.LoadScene("Day3House");
                }
                if (currentPlayerData.CurrentDay == 4)
                {
                    SceneManager.LoadScene("Day4House");
                }
                if (currentPlayerData.CurrentDay == 5)
                {
                    SceneManager.LoadScene("Day5House");
                }
                if (currentPlayerData.CurrentDay == 6)
                {
                    SceneManager.LoadScene("Day6House");
                }
                

            }
            else
            {
                Debug.Log("No save file found. Starting new game.");
                CreateNewGame();
                
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading game: " + e.Message);
            CreateNewGame();
            
        }
    }

    // Create new game data
    public void CreateNewGame()
    {
        currentPlayerData = new PlayerData()
        {
            Karma = 10,
            CurrentDay = 0,
            hasMap = false,
            hasScarf = false,
            returnedScarf = false,
            hasOilLantern = false,
            hasHeirloomAxe = false,
            returnedHeirloomAxe = false,
            hasJournal = false,
            hasJeweleryBox = false,
            returnedJeweleryBox = false,
            hasWovenBasket = false,
            hasWarMedal = false,
            hasHairLocket = false,
            returnedHairLocket = false,
            hasWhiteSariScrap = false,
            hasDogTags = false,
            hasReligiousIcon = false,
            hasDoll = false,
            returnedDoll = false,
            hasWaterloggedPistol = false,
            hasFamilyPhoto = false,
            returnedFamilyPhoto = false,
            hasPrayerBeads = false,
            hasWoodenAnimals = false
        };
        SceneManager.LoadScene("Tutorial");
    }

    // Get current player data (for other scripts to access)
    public PlayerData GetPlayerData()
    {
        if (currentPlayerData == null)
        {
            LoadGame(); // Try to load, or create new if no save exists
        }
        return currentPlayerData;
    }

    // Set player data (for when you want to update from other scripts)
    public void SetPlayerData(PlayerData data)
    {
        currentPlayerData = data;
    }

    // Check if save file exists
    public bool SaveExists()
    {
        return File.Exists(savePath);
    }

    // Delete save file (for reset functionality)
    public void DeleteSave()
    {
        try
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                currentPlayerData = null;
                Debug.Log("Save file deleted!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error deleting save file: " + e.Message);
        }
    }

    // For debugging: print save path
    public void PrintSavePath()
    {
        Debug.Log("Save path: " + savePath);
    }
}