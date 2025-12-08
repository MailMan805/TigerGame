using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
   
    public bool finalLevel = false;

    private void Start()
    {
        if(finalLevel)
        {
            GameManager.instance.OnMainLevelLoaded.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var GO = other.gameObject;

        if (GO.tag != "Player") return;

        if (finalLevel)
        {
            GameManager.instance.currentDay = 9;
            GameManager.instance.LeaveHouse?.Invoke();
            NewPlayerMovement.Instance.enabled = false;
            enabled = false; // Deactivate Level exit
            
        }
        else if (LevelManager.Instance.collectedAllBodies)
        {
            GameManager.instance.LeaveLevel?.Invoke();
            NewPlayerMovement.Instance.enabled = false;
            enabled = false; // Deactivate Level exit
        }
    }
}
