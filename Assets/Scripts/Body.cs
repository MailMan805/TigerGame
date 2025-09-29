using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
<<<<<<< HEAD
    public bool TEST = false;
    public bool withinRange = false;
=======
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")

    private void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (withinRange) { CollectBody(); }
=======
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(GameManager.instance.PlayerInteractButton))
            {
                CollectBody();
            }
>>>>>>> parent of f5f51a1 (Revert "Merge branch 'VignetteBasedOnMashing' into Lily_Branch_2")
        }
    }

    void CollectBody()
    {
        ItemCollectionScript.instance.CollectItem();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            withinRange = false;
        }
    }
}
