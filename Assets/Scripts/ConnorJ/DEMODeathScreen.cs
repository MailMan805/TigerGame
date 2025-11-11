using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEMODeathScreen : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        GameManager.instance.OnDeath.AddListener(ShowDeathScreen);
    }

    void ShowDeathScreen()
    {
        Debug.Log("SENT");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canvas.enabled = true;
    }

    public void QuitButton()
    {
        GameManager.instance.QuitGame();
    }

    public void RetryButton()
    {
        GameManager.instance.ResetGame.Invoke();
        SceneManager.LoadScene(2);
    }
}
