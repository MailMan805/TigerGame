using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject pauseMenuPrefab;

    public Slider musicVolume;
    public Slider sfxVolume;
    public static bool gameIsPaused;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        pauseMenuPrefab.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().name != "MAIN MENU")
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

    public void StartNewGame()
    {
        //Update later when saves and scenes are established
        AudioManager.Instance.PlayMusic("Tutorial Music");
        SceneManager.LoadScene("Day1House");
    }

    public void LoadGame()
    {
        AudioManager.Instance.PlayMusic("Neutral Ambience");
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
        Cursor.lockState = CursorLockMode.None;
        NewPlayerMovement.Instance.GetComponentInChildren<Canvas>().enabled = false;
        NewPlayerMovement.Instance.playerControls.Disable();
    }

    public void ResumeGame()
    {
        pauseMenuPrefab.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        NewPlayerMovement.Instance.GetComponentInChildren<Canvas>().enabled = true;
        NewPlayerMovement.Instance.playerControls.Enable();
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MAIN MENU");
        AudioManager.Instance.PlayMusic("Menu Theme");
    }
}
