using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManger : MonoBehaviour
{

    public Slider musicVolume;
    public Slider sfxVolume;

    public SaveAndLoadManager saver;

    private void Start()
    {
        saver = FindAnyObjectByType<SaveAndLoadManager>();
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
        saver.LoadGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void CreateNewGame()
    {
        AudioManager.Instance.PlayMusic("Tutorial Music");
        SceneManager.LoadScene("Day1House");
        saver.CreateNewGame();
    }
}
