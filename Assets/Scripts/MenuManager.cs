using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenuPrefab;
    public static bool gameIsPaused;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PlayGame()
    {
<<<<<<< Updated upstream
        SceneManager.LoadScene("JadenTigerTest");
=======
        //Update later when saves and scenes are established
        SceneManager.LoadScene("Main Level");
    }

    public void LoadGame()
    {
        //load save file
>>>>>>> Stashed changes
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void PauseGame()
    {
        pauseMenuPrefab.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPrefab.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}
