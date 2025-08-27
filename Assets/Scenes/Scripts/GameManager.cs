using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManager instance;

    void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
        }
        instance = this;
    }
}
