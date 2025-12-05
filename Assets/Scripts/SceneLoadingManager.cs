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
    LEVELSIX,
    LEVELSEVEN,
    CUTSCENE,
    HOUSE1,
    HOUSE2,
    HOUSE3,
    HOUSE4,
    HOUSE5,
    HOUSE6,
    TUTORIAL = 15
}
public class SceneLoadingManager : MonoBehaviour
{
    float SecondsBeforeLoading = 1f;

    public void LoadNextLevel()
    {
        if (GameManager.instance.currentDay >= (int)SceneID.CUTSCENE)
        {
            print("Loading Final Night");
            StartCoroutine(LoadNewScene((int)SceneID.CUTSCENE));
            return;
        }

        print("Loading Night " + GameManager.instance.currentDay);
        switch (GameManager.instance.currentDay)
        {
            case 0:
                Debug.Log("TUTORIAL - Is GameManager at right currentDay?");
                StartCoroutine(LoadNewScene((int)SceneID.TUTORIAL));
                break;
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
            case 6:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELSIX));
                break;
            case 7:
                StartCoroutine(LoadNewScene((int)SceneID.LEVELSEVEN));
                break;
        }
        
    }

    public void LoadHouse()
    {
        print("Loading House...");
        switch (GameManager.instance.currentDay)
        {
            case 1:
                StartCoroutine(LoadNewScene((int)SceneID.HOUSE1));
                break;
            case 2:
                StartCoroutine(LoadNewScene((int)SceneID.HOUSE2));
                break;
            case 3:
                StartCoroutine(LoadNewScene((int)SceneID.HOUSE3));
                break;
            case 4:
                StartCoroutine(LoadNewScene((int)SceneID.HOUSE4));
                break;
            case 5:
                StartCoroutine(LoadNewScene((int)SceneID.HOUSE5));
                break;
            case 6:
                StartCoroutine(LoadNewScene((int)SceneID.HOUSE6));
                break;
        }
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
