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
    public static PlayerData currentPlayerData;

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

            // Load existing data if available
            if (File.Exists(savePath))
            {
                LoadGameData();
            }
        }
    }

    private void Start()
    {
        // Ensure we have player data
        if (currentPlayerData == null)
        {
            GetPlayerData();
        }
    }

    private void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Auto-save when entering certain levels
        if (ShouldAutoSave(scene.name))
        {
            // Add a small delay to ensure everything is initialized
            StartCoroutine(DelayedAutoSave());
        }
    }

    private IEnumerator DelayedAutoSave()
    {
        yield return new WaitForSeconds(0.1f);
        AutoSave();
    }

    // Define which levels should trigger auto-save
    private bool ShouldAutoSave(string sceneName)
    {
        // Add the names of levels where you want to auto-save
        string[] autoSaveLevels = { "Tutorial", "Day2House", "Day3House", "Day4House", "Day5House", "Day6House" };

        foreach (string level in autoSaveLevels)
        {
            if (sceneName == level)
            {
                if (GameManager.DiedInLevel == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Auto-save method
    public void AutoSave()
    {
        if (currentPlayerData != null)
        {
            // Update current day from GameManager if available
            if (GameManager.instance != null)
            {
                currentPlayerData.CurrentDay = GameManager.instance.currentDay;
            }

            // Update current item from ItemCollectionScript if available
            if (ItemCollectionScript.instance != null)
            {
                currentPlayerData.CurrentItem = GameManager.instance.currentItem;
            }

            SaveGame();
            Debug.Log("Game auto-saved!");
        }
        else
        {
            Debug.LogWarning("Cannot auto-save: currentPlayerData is null!");
        }
    }

    public void SaveKarma(int karma)
    {
        if (currentPlayerData != null)
        {
            currentPlayerData.Karma = karma;
            //SaveGame(); 
            Debug.Log($"Saved Karma: {karma}");
        }
        else
        {
            Debug.LogWarning("Cannot save karma: currentPlayerData is null!");
        }
    }

    public void SaveCurrentItem(int item)
    {
        if (currentPlayerData != null)
        {
            currentPlayerData.CurrentItem = item;
            //SaveGame(); 
            Debug.Log($"Saved CurrentItem: {item}");
        }
        else
        {
            Debug.LogWarning("Cannot save item: currentPlayerData is null!");
        }
    }

    public void SaveCurrentDay(int day)
    {
        if (currentPlayerData != null)
        {
            currentPlayerData.CurrentDay = day;
            //SaveGame(); // UNCOMMENTED - This actually writes to the file
            Debug.Log($"Saved CurrentDay: {day}");
        }
        else
        {
            Debug.LogWarning("Cannot save day: currentPlayerData is null!");
        }
    }

    public void SaveItemStatus(string itemName, bool value)
    {
        if (currentPlayerData != null)
        {
            switch (itemName)
            {
                // Map
                case "hasMap":
                    currentPlayerData.hasMap = value;
                    break;

                // Scarf items
                case "hasScarf":
                    currentPlayerData.hasScarf = value;
                    break;
                case "returnedScarf":
                    currentPlayerData.returnedScarf = value;
                    break;

                // Oil Lantern
                case "hasOilLantern":
                    currentPlayerData.hasOilLantern = value;
                    break;

                // Heirloom Axe items
                case "hasHeirloomAxe":
                    currentPlayerData.hasHeirloomAxe = value;
                    break;
                case "returnedHeirloomAxe":
                    currentPlayerData.returnedHeirloomAxe = value;
                    break;

                // Journal
                case "hasJournal":
                    currentPlayerData.hasJournal = value;
                    break;

                // Jewelery Box items
                case "hasJeweleryBox":
                    currentPlayerData.hasJeweleryBox = value;
                    break;
                case "returnedJeweleryBox":
                    currentPlayerData.returnedJeweleryBox = value;
                    break;

                // Woven Basket
                case "hasWovenBasket":
                    currentPlayerData.hasWovenBasket = value;
                    break;

                // War Medal
                case "hasWarMedal":
                    currentPlayerData.hasWarMedal = value;
                    break;

                // Hair Locket items
                case "hasHairLocket":
                    currentPlayerData.hasHairLocket = value;
                    break;
                case "returnedHairLocket":
                    currentPlayerData.returnedHairLocket = value;
                    break;

                // White Sari Scrap
                case "hasWhiteSariScrap":
                    currentPlayerData.hasWhiteSariScrap = value;
                    break;

                // Dog Tags
                case "hasDogTags":
                    currentPlayerData.hasDogTags = value;
                    break;

                // Religious Icon
                case "hasReligiousIcon":
                    currentPlayerData.hasReligiousIcon = value;
                    break;

                // Doll items
                case "hasDoll":
                    currentPlayerData.hasDoll = value;
                    break;
                case "returnedDoll":
                    currentPlayerData.returnedDoll = value;
                    break;

                // Waterlogged Pistol
                case "hasWaterloggedPistol":
                    currentPlayerData.hasWaterloggedPistol = value;
                    break;

                // Family Photo items
                case "hasFamilyPhoto":
                    currentPlayerData.hasFamilyPhoto = value;
                    break;
                case "returnedFamilyPhoto":
                    currentPlayerData.returnedFamilyPhoto = value;
                    break;

                // Prayer Beads
                case "hasPrayerBeads":
                    currentPlayerData.hasPrayerBeads = value;
                    break;

                // Wooden Animals
                case "hasWoodenAnimals":
                    currentPlayerData.hasWoodenAnimals = value;
                    break;

                default:
                    Debug.LogWarning($"Item name '{itemName}' not recognized!");
                    return;
            }
            //SaveGame(); // UNCOMMENTED - This actually writes to the file
            Debug.Log($"Saved {itemName}: {value}");
        }
        else
        {
            Debug.LogWarning("Cannot save item status: currentPlayerData is null!");
        }
    }

    // Save game data to JSON file
    public void SaveGame()
    {
        try
        {
            if (currentPlayerData == null)
            {
                Debug.LogWarning("Cannot save: currentPlayerData is null!");
                return;
            }

            string jsonData = JsonUtility.ToJson(currentPlayerData, true);
            File.WriteAllText(savePath, jsonData);
            Debug.Log("Game saved successfully to: " + savePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving game: " + e.Message);
        }
    }

    // Load game data without changing scenes
    public void LoadGameData()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string jsonData = File.ReadAllText(savePath);
                currentPlayerData = JsonUtility.FromJson<PlayerData>(jsonData);
                Debug.Log("Game data loaded successfully!");
            }
            else
            {
                Debug.Log("No save file found.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error loading game data: " + e.Message);
        }
    }

    // Load game and change scene
    public void LoadGame()
    {
        try
        {
            if (File.Exists(savePath))
            {
                string jsonData = File.ReadAllText(savePath);
                currentPlayerData = JsonUtility.FromJson<PlayerData>(jsonData);
                Debug.Log("Game loaded successfully!");

                // Load the appropriate scene based on current day
                if (currentPlayerData.CurrentDay >= 0 && currentPlayerData.CurrentDay <= 6)
                {
                    string sceneName = GetSceneNameForDay(currentPlayerData.CurrentDay);
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    Debug.LogWarning("Invalid CurrentDay value: " + currentPlayerData.CurrentDay);
                    SceneManager.LoadScene("Tutorial");
                }

                GameManager.instance.currentDay = currentPlayerData.CurrentDay;
                GameManager.instance.currentItem = currentPlayerData.CurrentItem;
                GameManager.instance.Karma = currentPlayerData.Karma;
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

    private string GetSceneNameForDay(int day)
    {
        switch (day)
        {
            case 0: return "Tutorial";
            case 1: return "Day1House";
            case 2: return "Day2House";
            case 3: return "Day3House";
            case 4: return "Day4House";
            case 5: return "Day5House";
            case 6: return "Day6House";
            default: return "Tutorial";
        }
    }

    // Create new game data
    public void CreateNewGame()
    {
        currentPlayerData = new PlayerData()
        {
            Karma = 10,
            CurrentDay = 0,
            CurrentItem = 0,
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

        SaveGame(); // Save the new game data immediately
        SceneManager.LoadScene("Tutorial");
    }

    // Get current player data (for other scripts to access)
    public PlayerData GetPlayerData()
    {
        if (currentPlayerData == null)
        {
            if (File.Exists(savePath))
            {
                LoadGameData();
            }
            else
            {
                CreateNewGame();
            }
        }
        return currentPlayerData;
    }

    // Set player data (for when you want to update from other scripts)
    public void SetPlayerData(PlayerData data)
    {
        currentPlayerData = data;
        //SaveGame(); // Save immediately after setting
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

    // For debugging: print save path and current data
    public void PrintSaveInfo()
    {
        Debug.Log("Save path: " + savePath);
        if (currentPlayerData != null)
        {
            Debug.Log("Current Day: " + currentPlayerData.CurrentDay);
            Debug.Log("Current Item: " + currentPlayerData.CurrentItem);
            Debug.Log("Karma: " + currentPlayerData.Karma);
        }
        else
        {
            Debug.Log("currentPlayerData is null");
        }
    }
}