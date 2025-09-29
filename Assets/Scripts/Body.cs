using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(GameManager.instance.PlayerInteractButton))
            {
                CollectBody();
            }
        }
    }

    void CollectBody()
    {
        GameManager.instance.BodyCollected.Invoke();
        Destroy(gameObject);
    }
}
