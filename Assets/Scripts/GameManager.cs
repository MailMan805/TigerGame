using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum SceneID
{
    MAINMENU,
    HOUSE,
    MAINLEVEL,
    FINALLEVEL
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Range(0,7)] public int currentDay = 0;
    public int Karma { get; set; }


    public KeyCode PlayerInteractButton = KeyCode.Return;

    public UnityEvent BodyCollected;
    public UnityEvent<Night> OnMainLevelLoaded;
    public UnityEvent OnHouseLevelLoaded;
    public UnityEvent LeaveHouse;

    // NIGHT SETUP
    static Night NightOne = new Night(2);
    static Night NightTwo = new Night(2);
    static Night NightThree = new Night(3);
    static Night NightFour = new Night(3);
    static Night NightFive = new Night(4);
    static Night NightSix = new Night(4);

    const int FINAL_LEVEL_NUMBER = 7;

   


    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LeaveHouse.AddListener(IncrementDay);
    }

    private void OnLevelWasLoaded(int level)
    {
        switch (level) {
            case (int)SceneID.MAINMENU:
                break; 
            case (int)SceneID.HOUSE:
                break;
            case (int)SceneID.MAINLEVEL:
                MainLevelSetup();
                break;
            case (int)SceneID.FINALLEVEL:
                break;
        
        }
    }

    void MainLevelSetup()
    {
        switch (currentDay){
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

    void LoadNextLevel()
    {
        IncrementDay();
        if (currentDay >= FINAL_LEVEL_NUMBER)
        {
            SceneManager.LoadScene((int)SceneID.FINALLEVEL);
            return;
        }

        SceneManager.LoadScene((int)SceneID.MAINLEVEL);
    }

    void LoadHouse()
    {
        SceneManager.LoadScene((int)SceneID.HOUSE);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene((int)SceneID.MAINMENU);
    }

    void IncrementDay()
    {
        currentDay++;
    }

}
