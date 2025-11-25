using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var GO = other.gameObject;

        if (GO.tag != "Player") return;

        if (LevelManager.Instance.collectedAllBodies)
        {
            GameManager.instance.LeaveLevel.Invoke();
            AudioManager.Instance.PlayMusic("Neutral Ambience");
            enabled = false; // Deactivate Level exit
        }
    }
}
