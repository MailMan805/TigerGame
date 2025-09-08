using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentDay = 1;

    public KeyCode PlayerInteractButton = KeyCode.Return;

    public UnityEvent BodyCollected;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
