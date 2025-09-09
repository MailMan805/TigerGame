using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("JadenTigerTest");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void OpenPauseMenu()
    {
        SceneManager.LoadScene("PauseMenu");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
