using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("VARIABLES")]
    public float SecondsBeforeLoadingSceneDelay = 3f;
    [Range(0, 9)] public int currentDay = 1;
    public int bodyCount = 0;
    public KeyCode PlayerInteractButton = KeyCode.E;

    public int currentItem;
    public static bool DiedInLevel { get; set; } = false; // Persists only in House segments.

    public int tempCurrentItem;
    public int tempCurrentKarma;

    [Header("KARMA")]
    public int Karma = 10;
    [SerializeField] private int MaxKarma = 20; // 8 - 12 Neutral, Starts at 10, <= 8 Negative, >= 12 Positive.
    public const float GOODENDINGKARMA = 0.6f;

    [HideInInspector] public bool inItemMenu = false;

    [Header("EVENTS")]
    public UnityEvent BodyCollected;
    public UnityEvent OnMainLevelLoaded;
    public UnityEvent OnHouseLevelLoaded;
    public UnityEvent LeaveHouse;
    public UnityEvent LeaveLevel;
    public UnityEvent ResetGame;
    public UnityEvent OnDeath;

    public UnityEvent GoodEnding;
    public UnityEvent BadEnding;

    // Manager Setup
    public SceneLoadingManager sceneLoadingManager { get; set; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        sceneLoadingManager = gameObject.AddComponent<SceneLoadingManager>();

        sceneLoadingManager.SetNewSecondDelay(SecondsBeforeLoadingSceneDelay);

        ResetGame.AddListener(ResetGameData);
        LeaveHouse.AddListener(LeavingHouse);
        LeaveLevel.AddListener(LeavingLevel);

        OnDeath.AddListener(DeathData);

        DontDestroyOnLoad(gameObject);
    }

    #region Karma

    /// <summary>
    /// Returns a float value between 0.0f~1.0f depending on Karma / MaxKarma
    /// </summary>
    /// <returns></returns>
    public float GetKarmaLevel()
    {
        return (float)Karma / (float)MaxKarma;
    }

    /// <summary>
    /// Returns true if Player's karma level is above good ending threshold.
    /// </summary>
    /// <returns></returns>
    public bool OnGoodEndingPath()
    {
        return GameManager.instance.Karma >= 12;
    }

    /// <summary>
    /// Increase/Decrease Karma by a certain amount. Will not go over MaxKarma.
    /// </summary>
    /// <param name="amountOfChange">Int that will add/subtract from karma</param>
    public void ChangeKarmaLevel(int amountOfChange)
    {
        Karma += amountOfChange;

        Karma = Mathf.Clamp(Karma, 0, MaxKarma);
    }

    #endregion

    public void IncrementDay()
    {
        currentDay++;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CommitEnding()
    {
        if (Karma >= 12)
        {
            GoodEnding.Invoke();
        } else
        {
            BadEnding.Invoke();
        }
    }

    void TestLoading()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            sceneLoadingManager.LoadMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            sceneLoadingManager.LoadHouse();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            sceneLoadingManager.LoadNextLevel();
        }
    }

    #region Data Updating
    void ResetGameData()
    {
        currentDay = 0;
        inItemMenu = false;

    }

    void LeavingHouse()
    {
        tempCurrentItem = currentItem;
        tempCurrentKarma = Karma;
        DiedInLevel = false;
        sceneLoadingManager.LoadNextLevel();
    }

    void LeavingLevel()
    {
        IncrementDay();
        sceneLoadingManager.LoadHouse();
    }

    void DeathData()
    {
        DiedInLevel = true;
        Karma = tempCurrentKarma;
        currentItem = tempCurrentItem;
        SaveAndLoadManager.Instance.LoadGameData();
        sceneLoadingManager.LoadHouse();
    }

    #endregion

    #region Context Menu Functions

    [ContextMenu("Decrease Karma")]
    private void DecreaseKarma()
    {
        ChangeKarmaLevel(-1);
    }

    [ContextMenu("Increase Karma")]
    private void IncreaseKarma()
    {
        ChangeKarmaLevel(1);
    }

    #endregion

}
