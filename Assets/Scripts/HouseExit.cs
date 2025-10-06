using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.LeaveHouse.Invoke();
        }
    }
}
