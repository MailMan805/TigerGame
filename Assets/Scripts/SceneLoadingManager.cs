using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneID
{
    MAINMENU,
    HOUSE,
    LEVELONE,
    LEVELTWO,
    LEVELTHREE,
    LEVELFOUR,
    LEVELFIVE,
    FINALLEVEL
}
public class SceneLoadingManager : MonoBehaviour
{

    private void Awake()
    {
        GameManager.instance.LeaveHouse.AddListener(LoadNextLevel);
    }

    public void LoadNextLevel()
    {
        GameManager.instance.IncrementDay();
        if (GameManager.instance.currentDay >= (int)SceneID.FINALLEVEL)
        {
            print("Loading Final Night");
            SceneManager.LoadScene((int)SceneID.FINALLEVEL);
            return;
        }

        print("Loading Night " + GameManager.instance.currentDay);
        switch (GameManager.instance.currentDay)
        {
            case 1:
                SceneManager.LoadScene((int)SceneID.LEVELONE);
                break;
            case 2:
                SceneManager.LoadScene((int)SceneID.LEVELTWO);
                break;
            case 3:
                SceneManager.LoadScene((int)SceneID.LEVELTHREE);
                break;
            case 4:
                SceneManager.LoadScene((int)SceneID.LEVELFOUR);
                break;
            case 5:
                SceneManager.LoadScene((int)SceneID.LEVELFIVE);
                break;
        }
        
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
