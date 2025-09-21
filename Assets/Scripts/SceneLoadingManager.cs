using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneID
{
    MAINMENU,
    HOUSE,
    MAINLEVEL,
    FINALLEVEL
}
public class SceneLoadingManager : MonoBehaviour
{
    const int FINAL_LEVEL_NUMBER = 7;

    private void Awake()
    {
        GameManager.instance.LeaveHouse.AddListener(LoadNextLevel);
    }

    public void LoadNextLevel()
    {
        GameManager.instance.IncrementDay();
        if (GameManager.instance.currentDay >= FINAL_LEVEL_NUMBER)
        {
            print("Loading Final Night");
            SceneManager.LoadScene((int)SceneID.FINALLEVEL);
            return;
        }

        print("Loading Night " + GameManager.instance.currentDay);
        SceneManager.LoadScene((int)SceneID.MAINLEVEL);
    }

    public void LoadHouse()
    {
        print("Loading House...");
        SceneManager.LoadScene((int)SceneID.HOUSE);
    }

    public void LoadMainMenu()
    {
        print("Loading Main Menu...");
        SceneManager.LoadScene((int)SceneID.MAINMENU);
    }

    
}
