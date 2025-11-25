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

            //added this line in for door noise when door - Conner W
            AudioManager.Instance.PlaySound("DoorOpen");

            GameManager.instance.LeaveHouse.Invoke();
            AudioManager.Instance.PlayMusic("Neutral Ambience");
            gameObject.SetActive(false);
            other.gameObject.GetComponent<NewPlayerMovement>().enabled = false;
        }
    }
}
