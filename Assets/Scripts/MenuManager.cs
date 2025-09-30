using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenuPrefab;

    public Slider musicVolume;
    public Slider sfxVolume;
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

        //For testing, remove later
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.PlayMusic("Test_Music");
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            AudioManager.Instance.PlayMusic("Test_Music2");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            AudioManager.Instance.PlaySound("Test_Bonk");
        }
    }

    public void StartNewGame()
    {
        //Update later when saves and scenes are established
        SceneManager.LoadScene("House");
    }

    public void LoadGame()
    {
        //load save file
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

    public void AdjustMusicVolume()
    {
        AudioManager.Instance.musicSource.volume = musicVolume.value;
    }

    public void AdjustSFXVolume()
    {
        AudioManager.Instance.soundEffectsSource.volume = sfxVolume.value;
    }

}
