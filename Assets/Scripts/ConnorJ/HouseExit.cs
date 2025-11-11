using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseExit : MonoBehaviour
{
    bool touched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !touched)
        {
            print("Leaving lol");
            touched = true;
            GameManager.instance.LeaveHouse.Invoke();
            gameObject.SetActive(false);
            other.gameObject.GetComponent<NewPlayerMovement>().enabled = false;
        }
    }
}
