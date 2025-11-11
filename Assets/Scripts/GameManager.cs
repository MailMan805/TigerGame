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
    [Range(0,7)] public int currentDay = 0;
    public int bodyCount = 0;
    public KeyCode PlayerInteractButton = KeyCode.E;
    public static bool DiedInLevel { get; set; } = false; // Persists only in House segments.

    [Header("KARMA")]
    public int Karma = 10;
    [SerializeField] private int MaxKarma = 20; // 8 - 12 Neutral, Starts at 10, <= 8 Negative, >= 12 Positive.

    [HideInInspector] public bool inItemMenu = false;

    [Header("EVENTS")]
    public UnityEvent BodyCollected;
    public UnityEvent OnMainLevelLoaded;
    public UnityEvent OnHouseLevelLoaded;
    public UnityEvent LeaveHouse;
    public UnityEvent ResetGame;
    public UnityEvent OnDeath;

    // Manager Setup
    public SceneLoadingManager sceneLoadingManager { get; set; }

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        currentDay = 0;

        sceneLoadingManager = gameObject.AddComponent<SceneLoadingManager>();

        sceneLoadingManager.SetNewSecondDelay(SecondsBeforeLoadingSceneDelay);

        ResetGame.AddListener(ResetGameData);
        LeaveHouse.AddListener(LeavingHouse);

        OnDeath.AddListener(DeathData);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Returns a float value between 0.0f~1.0f depending on Karma / MaxKarma
    /// </summary>
    /// <returns></returns>
    public float GetKarmaLevel()
    {
        return Karma / MaxKarma;
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

    public void IncrementDay()
    {
        currentDay++;
    }

    public void QuitGame()
    {
        Application.Quit();
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

    void ResetGameData()
    {
        currentDay = 0;
        inItemMenu = false;

    }

    void LeavingHouse()
    {
        IncrementDay();
        DiedInLevel = false;
        sceneLoadingManager.LoadNextLevel();
    }

    void DeathData()
    {
        DiedInLevel = true;
        currentDay--;
        sceneLoadingManager.LoadHouse();
    }

}
