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
    float SecondsBeforeLoading = 1f;

    public void LoadNextLevel()
    {
        if (GameManager.instance.currentDay >= (int)SceneID.FINALLEVEL)
        {
            print("Loading Final Night");
            StartCoroutine(LoadNewScene((int)SceneID.FINALLEVEL));
            return;
        }

        print("Loading Night " + GameManager.instance.currentDay);
        switch (GameManager.instance.currentDay)
        {
            case 1:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELONE));
                break;
            case 2:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELTWO));
                break;
            case 3:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELTHREE));
                break;
            case 4:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELFOUR));
                break;
            case 5:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELFIVE));
                break;
        }
        
    }

    public void LoadHouse()
    {
        print("Loading House...");
        StartCoroutine(LoadNewScene((int)SceneID.HOUSE));
    }

    public void LoadMainMenu()
    {
        print("Loading Main Menu...");
        StartCoroutine(LoadNewScene((int)SceneID.MAINMENU));
    }

    IEnumerator LoadNewScene(int Scene)
    {
        yield return new WaitForSeconds(SecondsBeforeLoading);
        SceneManager.LoadScene(Scene);
    }

    public void SetNewSecondDelay(float newSeconds)
    {
        SecondsBeforeLoading = newSeconds;
    }

    
}
