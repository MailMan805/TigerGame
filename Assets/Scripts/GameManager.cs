using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Range(0,7)] public int currentDay = 0;
    public int bodyCount = 0;
    public int Karma = 10;
    public int MaxKarma = 20; // 8 - 12 Neutral, Starts at 10, <= 8 Negative, >= 12 Positive.

    public bool inItemMenu = false;


    public KeyCode PlayerInteractButton = KeyCode.Return;

    public UnityEvent BodyCollected;
    public UnityEvent<Night> OnMainLevelLoaded;
    public UnityEvent OnHouseLevelLoaded;
    public UnityEvent LeaveHouse;

    // NIGHT SETUP
    static Night NightOne = new Night(2, FogDensity.NONE);
    static Night NightTwo = new Night(2, FogDensity.VERYLIGHT);
    static Night NightThree = new Night(3, FogDensity.LIGHT);
    static Night NightFour = new Night(3, FogDensity.NORMAL);
    static Night NightFive = new Night(4, FogDensity.HEAVY);
    static Night NightSix = new Night(4, FogDensity.VERYHEAVY);

    // Manager Setup
    SceneLoadingManager sceneLoadingManager = new SceneLoadingManager();

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        currentDay = 0;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        TestLoading();
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level) {
            case (int)SceneID.MAINMENU:
                break; 
            case (int)SceneID.HOUSE:
                break;
            case (int)SceneID.MAINLEVEL:
                StartCoroutine(MainLevelSetup());
                break;
            case (int)SceneID.FINALLEVEL:
                break;
        
        }
    }

    IEnumerator MainLevelSetup()
    {
        yield return null; // Wait a frame

        switch (currentDay)
        {
            case 1:
                OnMainLevelLoaded.Invoke(NightOne);
                break;
            case 2:
                OnMainLevelLoaded.Invoke(NightTwo);
                break;
            case 3:
                OnMainLevelLoaded.Invoke(NightThree);
                break;
            case 4:
                OnMainLevelLoaded.Invoke(NightFour);
                break;
            case 5:
                OnMainLevelLoaded.Invoke(NightFive);
                break;
            case 6:
                OnMainLevelLoaded.Invoke(NightSix);
                break;
        }
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

        if (Karma > MaxKarma)
        {
            Karma = MaxKarma;
        }
        if (Karma < 0) {
            Karma = 0;
        }
    }

    public void IncrementDay()
    {
        currentDay++;
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
}
